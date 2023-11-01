using System;
using ArcaneSurvivorsClient.Locale;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class BattleReportUI : MonoBehaviour {
        [Serializable]
        public class Page1 {
            public BattleReportUI parent;
            public GameObject panel;
            public Animator animator;
            public Button nextButton;

            public PlayableCharacterCardUI characterCardUI;
            public WeaponStatisticsUI weaponStatisticsUI;

            public void Init() {
                nextButton.onClick.AddListener(() => {
                    parent.SetPage(2);
                });
            }

            public void SetVisible(bool visible) {
                panel.SetActive(visible);

                if (visible) {
                    UpdateUI();
                    animator.SetTrigger("Replay");
                }
            }

            public void UpdateUI() {
                characterCardUI.SetModel(PlayableCharacterManager.Instance.SelectedCharacter);
                weaponStatisticsUI.UpdateUI();
            }
        }

        [Serializable]
        public class Page2 {
            public BattleReportUI parent;
            public GameObject panel;
            public Animator animator;
            public Button prevButton;
            public Button returnToLobbyButton;

            public TextMeshProUGUI playtimeText;
            public TextMeshProUGUI killCountText;
            public TextMeshProUGUI damageDealtText;
            public TextMeshProUGUI mostDamageDealtText;
            public TextMeshProUGUI damageTakenText;
            public TextMeshProUGUI hpHealedText;
            public TextMeshProUGUI collectedGoldText;
            public TextMeshProUGUI purchasedWeaponsText;
            public TextMeshProUGUI rerollCountText;

            public void Init() {
                prevButton.onClick.AddListener(() => {
                    parent.SetPage(1);
                });
                returnToLobbyButton.onClick.AddListener(() => {
                    ConfirmDialog.Show("dialog.returnToLobby".ToLocale(), () => {
                        ScreenTransition.Show(ScreenTransitionType.AlphaFade, ScreenTransitionType.AlphaFade, () => {
                            parent.SetVisible(false);
                            GameManager.Instance.EndGameAndReturnToLobby();
                        });
                    });
                });
            }

            public void SetVisible(bool visible) {
                panel.SetActive(visible);

                if (visible) {
                    UpdateUI();
                    animator.SetTrigger("Replay");
                }
            }

            public void UpdateUI() {
                GameStatistics statistics = GameManager.Instance.PlayingGame?.Statistics;
                if (statistics == null) return;

                playtimeText.text = statistics.playtime.ToString(@"hh\:mm\:ss");
                killCountText.text = statistics.killCount.ToDisplayString(DisplayNumberType.WithComma);
                damageDealtText.text = statistics.damageDealt.ToDisplayString(DisplayNumberType.WithComma);
                mostDamageDealtText.text = statistics.mostDamageDealt.ToDisplayString(DisplayNumberType.WithComma);
                damageTakenText.text = statistics.damageTaken.ToDisplayString(DisplayNumberType.WithComma);
                hpHealedText.text = statistics.hpHealed.ToDisplayString(DisplayNumberType.WithComma);
                collectedGoldText.text = statistics.collectedGold.ToDisplayString(DisplayNumberType.WithComma);
                purchasedWeaponsText.text = statistics.purchasedWeapons.ToDisplayString(DisplayNumberType.WithComma);
                rerollCountText.text = statistics.rerollCount.ToDisplayString(DisplayNumberType.WithComma);
            }
        }

        public static BattleReportUI Instance { get; private set; }

        public GameObject panel;

        public Page1 page1;
        public Page2 page2;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            page1.Init();
            page2.Init();

            SetVisible(false);
        }

        public void SetVisible(bool visible) {
            panel.SetActive(visible);

            if (visible)
                SetPage(1);
        }

        public void SetPage(int page) {
            page1.SetVisible(page == 1);
            page2.SetVisible(page == 2);
        }
    }
}