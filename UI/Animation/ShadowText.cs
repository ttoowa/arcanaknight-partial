using System;
using TMPro;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    [RequireComponent(typeof(TextMeshProUGUI))]
    [ExecuteInEditMode]
    public class ShadowText : MonoBehaviour {
        public TextMeshProUGUI refText;

        private TextMeshProUGUI text;

        private void Start() {
            text = GetComponent<TextMeshProUGUI>();
        }

        private void LateUpdate() {
            if (!Application.isPlaying) {
                if (text == null)
                    text = GetComponent<TextMeshProUGUI>();
            }

            if (refText == null) return;

            if (text.text != refText.text)
                text.text = refText.text;
        }
    }
}