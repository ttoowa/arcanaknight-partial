using System;
using System.Collections;
using ArcaneSurvivorsClient.Locale;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class StageResultUI : MonoBehaviour {
        public struct ClearStageModel {
            public int difficultyLevel;
            public int stageNum;
            public StageResultGrade grade;
            public int killCount;
            public int collectedGold;
            public int inventoryGold;
        }

        public struct GameOverModel {
            public int difficultyLevel;
            public int stageNum;
            public int accRewardSp;
        }

        public struct ClearActModel {
            public int difficultyLevel;
            public string contentName;
            public int accRewardSp;
            public int? unlockedDifficultyLevel;
        }

        [Serializable]
        public class ClearStageUI {
            public GameObject panel;
            public Animator animator;

            public TextMeshProUGUI killCountText;
            public TextMeshProUGUI collectedGoldText;
            public TextMeshProUGUI inventoryGoldText;

            public Button continueButton;

            public void Init() {
                continueButton.onClick.AddListener(() => {
                    if (GameManager.Instance.PlayingGame == null) return;

                    ScreenTransition.Show(ScreenTransitionType.SlideGradient, ScreenTransitionType.SlideGradient,
                        () => {
                            GameManager.Instance.PlayingGame.NextStage();
                            Instance.SetVisible(false);
                        });
                });
            }

            public void SetModel(ClearStageModel model) {
                killCountText.text = $"{model.killCount}";
                collectedGoldText.text = $"{model.collectedGold}";
                inventoryGoldText.text = $"{model.inventoryGold}";
            }

            public void SetVisible(bool visible) {
                panel.SetActive(visible);

                if (visible)
                    animator.SetTrigger("Replay");
            }
        }

        [Serializable]
        public class GameOverUI {
            public GameObject panel;
            public Animator animator;

            public TextMeshProUGUI rewardSpText;

            public Button continueButton;

            public void Init() {
                continueButton.onClick.AddListener(() => {
                    Instance.SetVisible(false);

                    BattleReportUI.Instance.SetVisible(true);
                });
            }

            public void SetModel(GameOverModel model) {
                rewardSpText.text = $"{model.accRewardSp}";
            }

            public void SetVisible(bool visible) {
                panel.SetActive(visible);

                if (visible)
                    animator.SetTrigger("Replay");
            }
        }

        [Serializable]
        public class ClearActUI {
            public GameObject panel;
            public Animator animator;

            public TextMeshProUGUI accSpText;
            public GameObject unlockedDifficultyLevelPanel;
            public TextMeshProUGUI unlockedDifficultyLevelText;

            public Button continueButton;

            public void Init() {
                continueButton.onClick.AddListener(() => {
                    if (GameManager.Instance.PlayingGame == null) return;

                    ScreenTransition.Show(ScreenTransitionType.SlideGradient, ScreenTransitionType.SlideGradient,
                        () => {
                            Instance.SetVisible(false);
                            BattleReportUI.Instance.SetVisible(true);
                        });
                });
            }

            public void SetModel(ClearActModel model) {
                accSpText.text = $"{model.accRewardSp}";
                if (model.unlockedDifficultyLevel.HasValue)
                    unlockedDifficultyLevelText.text = $"{model.unlockedDifficultyLevel.Value}";

                unlockedDifficultyLevelPanel.SetActive(model.unlockedDifficultyLevel.HasValue);
            }

            public void SetVisible(bool visible) {
                panel.SetActive(visible);

                if (visible)
                    animator.SetTrigger("Replay");
            }
        }

        public static StageResultUI Instance { get; private set; }

        public GameObject panel;

        public LocaleText difficultyLevelText;
        public TextMeshProUGUI stageNumText;
        public LocaleText resultText;
        public Color[] resultTextColors;

        public ClearStageUI clearStageUI;
        public GameOverUI gameOverUI;
        public ClearActUI clearActUI;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            clearStageUI.Init();
            gameOverUI.Init();
            clearActUI.Init();

            SetVisible(false);
        }

        public void SetVisible(bool visible) {
            panel.SetActive(visible);
        }

        public void ShowClearStage(ClearStageModel model) {
            clearStageUI.SetModel(model);

            ApplyDifficultyLevel(model.difficultyLevel);
            ApplyDayNum(model.stageNum);
            ApplyResultGrade(model.grade);

            gameOverUI.SetVisible(false);
            clearStageUI.SetVisible(true);
            clearActUI.SetVisible(false);
        }

        public void ShowGameOver(GameOverModel model) {
            gameOverUI.SetModel(model);

            ApplyDifficultyLevel(model.difficultyLevel);
            ApplyDayNum(model.stageNum);
            ApplyResultGrade(StageResultGrade.GameOver);

            clearStageUI.SetVisible(false);
            gameOverUI.SetVisible(true);
            clearActUI.SetVisible(false);
        }

        public void ShowClearAct(ClearActModel model) {
            clearActUI.SetModel(model);

            ApplyDifficultyLevel(model.difficultyLevel);
            ApplyContentName(model.contentName);
            ApplyResultGrade(StageResultGrade.ClearStage);

            clearStageUI.SetVisible(false);
            gameOverUI.SetVisible(false);
            clearActUI.SetVisible(true);
        }

        private void ApplyDifficultyLevel(int difficultyLevel) {
            difficultyLevelText.Parameters = new string[] { difficultyLevel.ToString() };
        }

        private void ApplyDayNum(int dayNum) {
            stageNumText.text = $"Stage {dayNum}";
        }

        private void ApplyContentName(string contentName) {
            stageNumText.text = contentName;
        }

        private void ApplyResultGrade(StageResultGrade grade) {
            string resultKey;
            string storyKey;
            switch (grade) {
                case StageResultGrade.ClearStage:
                    resultKey = "stageResult.resultText.clearStage";
                    resultText.Color = resultTextColors[0];
                    break;
                case StageResultGrade.GameOver:
                    resultKey = "stageResult.resultText.gameOver";
                    resultText.Color = resultTextColors[1];
                    break;
                case StageResultGrade.ClearAct:
                    resultKey = "stageResult.resultText.clearAct";
                    resultText.Color = resultTextColors[2];
                    break;
                default:
                    resultKey = "[ERROR]";
                    resultText.Color = resultTextColors[1];
                    LogBuilder.BuildException(LogType.Error, nameof(StageResultUI), "Invalid StageResultGrade");
                    break;
            }

            resultText.Key = resultKey;
        }
    }
}