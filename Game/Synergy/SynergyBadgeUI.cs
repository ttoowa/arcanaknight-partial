using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class SynergyBadgeUI : MonoBehaviour {
        [HideInInspector]
        public Synergy model;

        public Image slotImage;
        public Image iconImage;

        public void SetModel(Synergy model) {
            this.model = model;

            iconImage.sprite = model.icon;
        }
    }
}