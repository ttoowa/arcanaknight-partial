using System;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    public class MainMenuTabGroup {
        public string id;
        public MainMenuTabButton tabButton;
        public GameObject tabContent;

        public void SetSelected(bool isSelected) {
            tabButton.SetSelected(isSelected);
            tabContent.SetActive(isSelected);
        }
    }
}