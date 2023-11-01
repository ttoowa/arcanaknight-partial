using System;
using ArcaneSurvivorsClient.Locale;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class GameChapterCardUI : MonoBehaviour {
        public GameChapter Model { get; private set; }

        public Button panelButton;
        public LocaleText nameText;
        public GameObject selectedFrame;
        public GameObject lockedFrame;
        public GameObject workingContentFrame;

        private void Start() {
            panelButton.onClick.AddListener(() => {
                if (Model == null) return;

                GameModeSelector.Instance.SelectChapter(Model);
            });

            GameModeSelector.Instance.OnGameChapterSelected += OnGameChapterSelected;
        }

        private void OnDestroy() {
            GameModeSelector.Instance.OnGameChapterSelected -= OnGameChapterSelected;
        }

        public void SetModel(GameChapter model) {
            Model = model;

            nameText.Parameters = new[] { model.id.ToString() };

            if (model.isWorkingContent) {
                workingContentFrame.SetActive(true);
                lockedFrame.SetActive(false);
            } else {
                workingContentFrame.SetActive(false);
                lockedFrame.SetActive(!model.IsPlayable);
            }

            panelButton.interactable = model.IsPlayable;
        }

        public void SetSelected(bool selected) {
            selectedFrame.SetActive(selected);
        }

        private void OnGameChapterSelected(GameChapter chapter) {
            SetSelected(chapter == Model);
        }
    }
}