using System;
using ArcaneSurvivorsClient.Locale;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class SynergyBuffDetailUI : MonoBehaviour {
        public static SynergyBuffDetailUI Instance { get; private set; }

        private SynergyBundle model;

        public GameObject panel;
        public Image iconImage;
        public LocaleText synergyNameText;
        public LocaleText buffNameText;
        public RectTransform descriptionArea;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            SetVisible(false);
        }

        public void SetModel(SynergyBundle model) {
            this.model = model;

            iconImage.sprite = model.synergy.buffIcon;
            synergyNameText.Key = model.synergy.name;
            buffNameText.Key = model.synergy.buffName;

            descriptionArea.ClearChilds();
            foreach (SynergyBuffSpec buffSpec in model.synergy.buffSpecs) {
                SynergyBuffDescriptionUI descUI = SynergyResource.Instance.synergyBuffDescriptionPrefab
                    .Instantiate(descriptionArea).Get<SynergyBuffDescriptionUI>();

                SynergyBundle buffBundle = new(model.synergy);
                buffBundle.SetLevel(buffSpec.conditionLevel);
                descUI.SetModel(buffBundle, buffSpec);

                if (model.Level < buffSpec.conditionLevel)
                    descUI.SetActive(false);
            }
        }

        public void SetVisible(bool visible) {
            panel.SetActive(visible);
        }
    }
}