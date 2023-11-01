using System;
using System.Linq;
using ArcaneSurvivorsClient.Locale;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class SynergyBuffDescriptionUI : MonoBehaviour {
        private SynergyBundle model;

        public CanvasGroup canvasGroup;
        public SynergyBundleUI bundleUI;
        public LocaleText descriptionText;

        private void Awake() {
            SetActive(true);
        }

        public void SetModel(SynergyBundle model, SynergyBuffSpec buffSpec) {
            this.model = model;
            bundleUI.SetModel(model);
            descriptionText.Key = model.synergy.buffDescription;
            descriptionText.Parameters = buffSpec.buffValues.Select(x => x.ToString()).ToArray();
        }

        public void SetActive(bool active) {
            canvasGroup.alpha = active ? 1 : 0.2f;
        }
    }
}