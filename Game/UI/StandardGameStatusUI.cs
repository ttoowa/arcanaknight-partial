using System;
using ArcaneSurvivorsClient.Locale;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class StandardGameStatusUI : MonoBehaviour {
        public GameObject panel;

        public TextMeshProUGUI dayNumText;
        public LocaleText placeText;
        public AssetText goldAssetText;

        public GuageUI playerHpGuage;
        public TextMeshProUGUI playerHpText;
        public Image characterProfileImage;

        public Button pauseButton;

        private void Awake() {
        }

        private void Start() {
            SetVisible(false);

            GameManager.Instance.PreGameStarted += OnPreGameStarted;
            GameManager.Instance.GameStarted += OnGameStarted;
            GameManager.Instance.GameEnded += OnGameEnded;

            pauseButton.onClick.AddListener(() => {
                PauseManager.Instance.Pause();
            });
        }

        private void Update() {
        }

        public void SetVisible(bool visible) {
            panel.SetActive(visible);
        }

        private void OnPreGameStarted(IGame game) {
            game.GoldAssetChanged += OnGoldChanged;
            game.PlayerHpChanged += OnPlayerHpChanged;
            characterProfileImage.sprite = PlayableCharacterManager.Instance.SelectedCharacter.slotIllust;

            switch (game.RuleType) {
                case GameRuleType.Standard:
                    StandardGame standardGame = game as StandardGame;

                    standardGame.PhaseChanged += OnPhaseChanged;
                    standardGame.DayChanged += OnDayChanged;

                    SetVisible(true);
                    break;
            }
        }

        private void OnGameStarted(IGame game) {
            StageChangeDirectingUI.Show(game.CurrentStage.map.name, game.CurrentStage.Name, () => {
            });
        }

        private void OnPlayerHpChanged(float hp, float maxhp) {
            playerHpGuage.SetValue(Mathf.Clamp01(hp / maxhp));
            playerHpText.text = $"<color=#EF4E2B>{hp.ToString("0")}</color>/{maxhp.ToString("0")}";
        }

        private void OnGameEnded(IGame game) {
            game.GoldAssetChanged -= OnGoldChanged;
            game.PlayerHpChanged -= OnPlayerHpChanged;

            switch (game.RuleType) {
                case GameRuleType.Standard:
                    StandardGame standardGame = game as StandardGame;

                    standardGame.PhaseChanged -= OnPhaseChanged;
                    standardGame.DayChanged -= OnDayChanged;

                    SetVisible(false);
                    break;
            }
        }

        private void OnPhaseChanged(GamePhase phase, StandardGame.StageInfo stageInfo) {
            switch (phase) {
                case GamePhase.None:
                    SetVisible(false);
                    break;
                case GamePhase.Day:
                    SetVisible(true);
                    placeText.Key = "map.shop";
                    break;
                case GamePhase.Night:
                    SetVisible(true);
                    placeText.Key = stageInfo.currentStage.map.name;
                    break;
                case GamePhase.NightResult:
                    SetVisible(false);
                    break;
            }
        }

        private void OnGoldChanged(float oldAmount, float newAmount) {
            goldAssetText.SetValue(Mathf.FloorToInt(newAmount));
        }

        private void OnDayChanged(StandardGame.StageInfo stageInfo) {
            dayNumText.text = $"Stage {stageInfo.currentStage.stageNum}";
        }
    }
}