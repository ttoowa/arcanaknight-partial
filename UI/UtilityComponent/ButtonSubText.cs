using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient {
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ButtonSubText : MonoBehaviour {
        public Button button;
        public Color disableColor;

        private Color originColor;
        private TextMeshProUGUI tmp;

        private void Awake() {
            tmp = GetComponent<TextMeshProUGUI>();

            originColor = tmp.color;
        }

        private void Update() {
            tmp.color = button.interactable ? originColor : disableColor;
        }
    }
}