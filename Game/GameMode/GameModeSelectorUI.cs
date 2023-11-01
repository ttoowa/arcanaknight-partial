using System;
using System.Collections.Generic;
using ArcaneSurvivorsClient.Locale;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class GameModeSelectorUI : MonoBehaviour {
        private interface ISelectorUI {
            void SetVisible(bool visible);

            void Init(GameModeSelectorUI ownerUI);
        }

        // Mode selector
        [Serializable]
        public class ModeSelector : ISelectorUI {
            private GameModeSelectorUI ownerUI;

            public GameObject panel;

            public GameObject modeCardPrefab;
            public RectTransform modeCardArea;
            public LocaleText modeDescText;
            public Button nextButton;

            public void Init(GameModeSelectorUI ownerUI) {
                this.ownerUI = ownerUI;

                GameModeSelector.Instance.OnGameModeSelected += OnGameModeSelected;

                GameModeSelector.Instance.SelectMode(GameModeManager.Instance.modeLibrary.dataObjects[0]);

                nextButton.onClick.AddListener(() => {
                    ownerUI.AddFlow(1);
                });
            }

            public void SetVisible(bool visible) {
                panel.SetActive(visible);

                if (visible) {
                    CreateCardUIs();
                    GameModeSelector.Instance.SelectMode(GameModeManager.Instance.modeLibrary.dataObjects[0]);
                }
            }

            private void CreateCardUIs() {
                int index = 0;
                modeCardArea.ClearChilds();
                foreach (GameMode model in GameModeManager.Instance.modeLibrary.dataObjects) {
                    GameModeCardUI modeCard = modeCardPrefab.Instantiate(modeCardArea).Get<GameModeCardUI>();
                    modeCard.SetModel(model);
                    modeCard.SetSelected(index == 0);
                    ++index;
                }
            }

            private void OnGameModeSelected(GameMode mode) {
                modeDescText.Key = mode.desc;
            }
        }

        [Serializable]
        public class ActSelector : ISelectorUI {
            private GameModeSelectorUI ownerUI;

            public GameObject panel;

            public GameObject actCardPrefab;
            public RectTransform actCardArea;
            public Button nextButton;

            public void Init(GameModeSelectorUI ownerUI) {
                this.ownerUI = ownerUI;

                nextButton.onClick.AddListener(() => {
                    ownerUI.AddFlow(1);
                });
            }

            public void SetVisible(bool visible) {
                panel.SetActive(visible);

                if (visible) {
                    CreateCardUIs();
                    GameModeSelector.Instance.SelectAct(
                        GameModeSelector.Instance.SelectedMode.actLibrary.dataObjects[0]);
                }
            }

            private void CreateCardUIs() {
                int index = 0;
                actCardArea.ClearChilds();
                foreach (GameAct act in GameModeSelector.Instance.SelectedMode.actLibrary.dataObjects) {
                    GameActCardUI actCard = actCardPrefab.Instantiate(actCardArea).Get<GameActCardUI>();
                    actCard.SetModel(act);

                    actCard.SetSelected(index == 0);
                    ++index;
                }
            }
        }

        [Serializable]
        public class ChapterSelector : ISelectorUI {
            private GameModeSelectorUI ownerUI;

            public GameObject panel;

            public GameObject chapterCardPrefab;
            public RectTransform chapterCardArea;
            public Button nextButton;

            public void Init(GameModeSelectorUI ownerUI) {
                this.ownerUI = ownerUI;

                nextButton.onClick.AddListener(() => {
                    ownerUI.AddFlow(1);
                });
            }

            public void SetVisible(bool visible) {
                panel.SetActive(visible);

                if (visible) {
                    CreateCardUIs();
                    GameModeSelector.Instance.SelectChapter(GameModeSelector.Instance.SelectedAct.chapterLibrary
                        .dataObjects[0]);
                }
            }

            private void CreateCardUIs() {
                int index = 0;
                chapterCardArea.ClearChilds();
                foreach (GameChapter model in GameModeSelector.Instance.SelectedAct.chapterLibrary.dataObjects) {
                    GameChapterCardUI chapterCard =
                        chapterCardPrefab.Instantiate(chapterCardArea).Get<GameChapterCardUI>();
                    chapterCard.SetModel(model);

                    chapterCard.SetSelected(index == 0);
                    ++index;
                }
            }
        }

        [Serializable]
        public class DifficultySelector : ISelectorUI {
            private GameModeSelectorUI ownerUI;

            public GameObject panel;

            private GameChapter SelectedChapter => GameModeSelector.Instance.SelectedChapter;
            private GameDifficultyState DifficultyState => SelectedChapter.difficultyState;

            public GameObject difficultyCardPrefab;
            public Transform difficultyCardArea;
            public LocaleText descText;
            public Button nextButton;

            private readonly List<GameDifficultyButton> difficultyButtonList = new();

            public void Init(GameModeSelectorUI ownerUI) {
                this.ownerUI = ownerUI;

                nextButton.onClick.AddListener(() => {
                    ScreenTransition.Show(ScreenTransitionType.AlphaFade, ScreenTransitionType.AlphaFade, () => {
                        MainMenuUI.Instance.SetVisible(false);
                        GameManager.Instance.StartGame(SelectedChapter);
                    }, fadeOutPostDelay: 0.6f);
                });
            }

            public void SetVisible(bool visible) {
                panel.SetActive(visible);

                if (visible) {
                    CreateButtons();

                    if (SelectedChapter != null) {
                        DifficultyState.DifficultySelected += OnDifficultySelected;
                        DifficultyState.DifficultyUnlocked += OnDifficultyUnlocked;
                        DifficultyState.SelectDifficulty(1);
                    }
                } else {
                    if (SelectedChapter != null) {
                        DifficultyState.DifficultySelected -= OnDifficultySelected;
                        DifficultyState.DifficultyUnlocked -= OnDifficultyUnlocked;
                    }
                }
            }

            private void CreateButtons() {
                difficultyCardArea.ClearChilds();
                difficultyButtonList.Clear();

                foreach (GameDifficulty difficulty in DifficultyState.library.dataObjects) {
                    GameDifficulty difficultyInstance = difficulty;
                    GameDifficultyButton button = difficultyCardPrefab.Instantiate(difficultyCardArea)
                        .Get<GameDifficultyButton>();
                    difficultyButtonList.Add(button);

                    button.SetModel(difficulty);
                    button.SetAvailable(DifficultyState.PlayableDifficultyLevel >= difficulty.level);
                    button.button.onClick.AddListener(() => {
                        DifficultyState.SelectDifficulty(difficultyInstance);
                    });
                }
            }

            private void OnDifficultySelected(GameDifficulty difficulty) {
                foreach (GameDifficultyButton button in difficultyButtonList) {
                    button.SetSelected(button.model.level == difficulty.level);
                }

                descText.Key = difficulty.desc;
            }

            private void OnDifficultyUnlocked(int level) {
                foreach (GameDifficultyButton button in difficultyButtonList) {
                    button.SetAvailable(button.model.level <= level);
                }
            }
        }

        public static GameModeSelectorUI Instance { get; private set; }

        private GameModeSelectFlow currentFlow;

        private ISelectorUI[] selectorUIs;

        public ModeSelector modeSelector;
        public ActSelector actSelector;
        public ChapterSelector chapterSelector;
        public DifficultySelector difficultySelector;

        public GameObject flowTitleArea;
        public LocaleText flowTitleText;
        public Button backButton;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            selectorUIs = new ISelectorUI[] {
                modeSelector,
                actSelector,
                chapterSelector,
                difficultySelector
            };
            foreach (ISelectorUI selectorUI in selectorUIs) {
                selectorUI.Init(this);
            }

            SetFlow(GameModeSelectFlow.Mode);

            backButton.onClick.AddListener(() => {
                AddFlow(-1);
            });
        }

        public void AddFlow(int step) {
            GameModeSelectFlow flow =
                (GameModeSelectFlow)Mathf.Clamp((int)currentFlow + step, 0, (int)GameModeSelectFlow.Difficulty);

            if (step > 0) {
                if (flow == GameModeSelectFlow.Act && GameModeSelector.Instance.SelectedMode.autoSelectFirstAct) {
                    ++flow;
                    GameModeSelector.Instance.SelectAct(
                        GameModeSelector.Instance.SelectedMode.actLibrary.dataObjects[0]);
                }

                if (flow == GameModeSelectFlow.Chapter &&
                    GameModeSelector.Instance.SelectedMode.autoSelectFirstChapter) {
                    ++flow;
                    GameModeSelector.Instance.SelectChapter(GameModeSelector.Instance.SelectedAct.chapterLibrary
                        .dataObjects[0]);
                }
            } else if (step < 0) {
                if (flow == GameModeSelectFlow.Chapter && GameModeSelector.Instance.SelectedMode.autoSelectFirstChapter)
                    --flow;

                if (flow == GameModeSelectFlow.Act && GameModeSelector.Instance.SelectedMode.autoSelectFirstAct)
                    --flow;
            }

            SetFlow(flow);
            UpdateVisible();
        }

        public void SetFlow(GameModeSelectFlow flow) {
            currentFlow = flow;
            UpdateVisible();
            MainMenuUI.Instance.ReplayContentEntryAnimation();
        }

        private void UpdateVisible() {
            foreach (ISelectorUI selectorUI in selectorUIs) {
                selectorUI.SetVisible(false);
            }

            flowTitleArea.SetActive(true);

            switch (currentFlow) {
                case GameModeSelectFlow.Mode:
                    modeSelector.SetVisible(true);
                    flowTitleArea.SetActive(false);
                    break;
                case GameModeSelectFlow.Act:
                    actSelector.SetVisible(true);
                    flowTitleText.Key = GameModeSelector.Instance.SelectedMode.name;
                    break;
                case GameModeSelectFlow.Chapter:
                    chapterSelector.SetVisible(true);
                    flowTitleText.Key = GameModeSelector.Instance.SelectedAct.name;
                    break;
                case GameModeSelectFlow.Difficulty:
                    difficultySelector.SetVisible(true);
                    flowTitleText.Key = "difficulty.selectorTitle";
                    break;
            }
        }
    }
}