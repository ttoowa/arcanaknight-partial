using System;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class PlayableCharacterSlotUI : MonoBehaviour {
        private PlayableCharacter model;

        public Button button;

        public GameObject content;
        public GameObject lockedContent;

        public Image slotPanel;
        public Image illustImage;

        public void SetAvailable(bool isAbailable) {
            content.SetActive(isAbailable);
            lockedContent.SetActive(!isAbailable);
            button.interactable = isAbailable;
        }

        public void SetModel(PlayableCharacter model) {
            this.model = model;

            illustImage.sprite = model.slotIllust;
            slotPanel.color = model.themeColor;
        }

        public void SetSelected(bool isSelected) {
            slotPanel.color = isSelected ? model.themeColor : Color.white;
        }
    }
}