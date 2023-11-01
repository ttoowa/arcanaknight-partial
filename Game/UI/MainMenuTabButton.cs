using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    [RequireComponent(typeof(Button))]
    public class MainMenuTabButton : MonoBehaviour {
        public event Action Clicked;

        public bool isAvailable = true;
        public Image panel;
        public TextMeshProUGUI text;
        public Image iconImage;
        public GameObject lockCover;
        public Sprite[] buttonSprites;
        public Color[] textColors;
        public Color[] iconColors;

        private Button button;


        private void Awake() {
            button = GetComponent<Button>();
            button.onClick.AddListener(() => {
                Clicked?.Invoke();
            });
        }

        private void Start() {
            SetAvailable(isAvailable);
        }

        public void SetAvailable(bool available) {
            button.interactable = available;
            lockCover.SetActive(!available);
        }

        public void SetSelected(bool isSelected) {
            panel.sprite = buttonSprites[isSelected ? 0 : 1];
            text.color = textColors[isSelected ? 0 : 1];
            iconImage.color = iconColors[isSelected ? 0 : 1];
        }
    }
}