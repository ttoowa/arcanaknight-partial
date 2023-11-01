using System;
using ArcaneSurvivorsClient.Locale;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class GameActCardUI : MonoBehaviour {
        public GameAct Mode { get; private set; }

        public Button panelButton;
        public Image thumbnailImage;
        public LocaleText nameText;
        public LocaleText titleText;
        public GameObject selectedFrame;
        public GameObject lockedFrame;

        private void Start() {
            panelButton.onClick.AddListener(() => {
                if (Mode == null) return;

                GameModeSelector.Instance.SelectAct(Mode);
            });

            GameModeSelector.Instance.OnGameActSelected += OnGameActSelected;
            OnGameActSelected(GameModeSelector.Instance.SelectedAct);
        }

        private void OnDestroy() {
            GameModeSelector.Instance.OnGameActSelected -= OnGameActSelected;
        }

        public void SetModel(GameAct model) {
            Mode = model;

            thumbnailImage.sprite = model.thumbnailSprite;
            nameText.Parameters = new[] { model.id.ToString() };
            titleText.Key = model.title;

            lockedFrame.SetActive(!model.isAvailable);
            panelButton.interactable = model.isAvailable;
        }

        public void SetSelected(bool selected) {
            selectedFrame.SetActive(selected);
        }

        private void OnGameActSelected(GameAct act) {
            SetSelected(act == Mode);
        }
    }
}