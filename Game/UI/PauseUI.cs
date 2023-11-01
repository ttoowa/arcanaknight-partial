using System;
using ArcaneSurvivorsClient.Analytics;
using ArcaneSurvivorsClient.Locale;
using Firebase.Analytics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class PauseUI : MonoBehaviour {
        public static PauseUI Instance { get; private set; }

        public GameObject panel;

        public TextMeshProUGUI playtimeText;
        public TextMeshProUGUI killCountText;
        public WeaponStatisticsUI weaponStatistics;

        public Button resumeButton;
        public Button returnToLobbyButton;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            SetVisible(false);

            PauseManager.Instance.PauseStateChanged += OnPauseStateChanged;
            PauseManager.Instance.PauseRejected += OnPauseRejected;

            resumeButton.onClick.AddListener(() => {
                PauseManager.Instance.Resume();
            });

            returnToLobbyButton.onClick.AddListener(() => {
                if (!GameManager.Instance.IsPlaying) return;

                ConfirmDialog.Show("dialog.returnToLobby".ToLocale(), () => {
                    if (!GameManager.Instance.IsPlaying) return;

                    ScreenTransition.Show(ScreenTransitionType.AlphaFade, ScreenTransitionType.AlphaFade, () => {
                        GameAnalytics.LogEvent("GameBroken", // 
                            new Parameter("content", GameManager.Instance.PlayingGame.ContentCode), // 
                            new Parameter("difficulty",
                                GameManager.Instance.PlayingGame.PlayingChapter.difficultyState.SelectedDifficulty
                                    .level), new Parameter("stage", GameManager.Instance.PlayingGame.CurrentStage.name),
                            new Parameter("character", PlayableCharacterManager.Instance.SelectedCharacter.name));

                        PauseManager.Instance.Resume();
                        GameManager.Instance.EndGameAndReturnToLobby();
                    });
                });
            });
        }

        private void OnPauseRejected(float leftCooltime) {
            ToastMessage.Show("alert.pauseCooltimeIsLeft".ToLocale(Mathf.CeilToInt(leftCooltime).ToString()));
        }

        private void OnPauseStateChanged(bool paused) {
            SetVisible(paused);
        }

        public void SetVisible(bool visible) {
            panel.SetActive(visible);

            if (visible)
                UpdateUI();
        }

        private void UpdateUI() {
            weaponStatistics.UpdateUI();

            if (GameManager.Instance.IsPlaying) {
                playtimeText.text = GameManager.Instance.PlayingGame.Statistics.playtime.ToString(@"hh\:mm\:ss");
                killCountText.text =
                    GameManager.Instance.PlayingGame.Statistics.killCount.ToDisplayString(DisplayNumberType.WithComma);
            } else {
                playtimeText.text = "00:00:00";
                killCountText.text = "0";
            }
        }
    }
}