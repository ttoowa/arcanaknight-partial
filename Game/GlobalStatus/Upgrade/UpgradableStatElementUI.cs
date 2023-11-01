using System;
using ArcaneSurvivorsClient.Locale;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class UpgradableStatElementUI : MonoBehaviour {
        public Image iconImage;
        public Button button;
        public TextMeshProUGUI levelText;
        public LocaleText nameText;
        public TextMeshProUGUI buffText;
        public TextMeshProUGUI priceText;

        public Action Clicked;

        private void Awake() {
            button.onClick.AddListener(() => {
                Clicked?.Invoke();
            });
        }

        public void SetModel(UpgradableStatBundle statBundle) {
            iconImage.sprite = statBundle.Define.iconSprite;
            nameText.Key = statBundle.Define.name;

            Clicked += () => {
                statBundle.LevelUp();
            };
            statBundle.LevelChanged += OnLevelChanged;
            OnLevelChanged(statBundle.level, statBundle);
        }

        private void OnLevelChanged(int level, UpgradableStatBundle statBundle) {
            bool isMaxLevel = level >= statBundle.Define.maxLevel;
            button.interactable = !isMaxLevel;
            levelText.text = isMaxLevel ? "MAX" : $"{level}";
            buffText.text = statBundle.CurrentValue.ToString();
            priceText.text = statBundle.CurrentPrice.ToString();
        }
    }
}