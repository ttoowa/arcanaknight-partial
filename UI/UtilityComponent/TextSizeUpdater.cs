using System;
using TMPro;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class TextSizeUpdater : MonoBehaviour {
        private TextMeshProUGUI tmp;
        private string renderText;
        private string originText;
        private int fixPhase;

        private void Awake() {
            fixPhase = 0;
            renderText = "";
        }

        private void Start() {
            tmp = GetComponent<TextMeshProUGUI>();
        }

        private void Update() {
            if (!tmp) return;

            if (renderText == tmp.text) {
                if (fixPhase == 0) {
                    fixPhase = 1;
                    renderText += " ";
                    tmp.text = renderText;
                } else if (fixPhase == 1) {
                    fixPhase = 2;
                    renderText = originText;
                    tmp.text = renderText;
                }
            }

            if (originText != tmp.text) {
                originText = tmp.text;
                renderText = originText;
                fixPhase = 0;
            }
        }
    }
}