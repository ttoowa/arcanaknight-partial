using System;
using ArcaneSurvivorsClient.Locale;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class GameModeCardUI : MonoBehaviour {
        public GameMode Model { get; private set; }

        public Button panelButton;
        public Image thumbnailImage;
        public LocaleText nameText;
        public GameObject selectedFrame;
        public GameObject unselectedFrame;
        public GameObject lockedFrame;


        private void Start() {
            panelButton.onClick.AddListener(() => {
                if (Model == null) return;

                GameModeSelector.Instance.SelectMode(Model);
            });

            GameModeSelector.Instance.OnGameModeSelected += OnGameModeSelected;
            OnGameModeSelected(GameModeSelector.Instance.SelectedMode);
        }

        private void OnDestroy() {
            GameModeSelector.Instance.OnGameModeSelected -= OnGameModeSelected;
        }

        public void SetModel(GameMode model) {
            Model = model;

            thumbnailImage.sprite = model.thumbnailSprite;
            nameText.Key = model.name;

            lockedFrame.SetActive(!model.isAvailable);
            panelButton.interactable = model.isAvailable;
        }

        public void SetSelected(bool selected) {
            selectedFrame.SetActive(selected);
            unselectedFrame.SetActive(!selected);
        }

        private void OnGameModeSelected(GameMode mode) {
            SetSelected(mode == Model);
        }
    }
}