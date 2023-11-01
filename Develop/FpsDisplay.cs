using TMPro;
using UnityEngine;

namespace ArcaneSurvivorsClient.Debugging {
    public class FpsDisplay : MonoBehaviour {
        public TextMeshProUGUI textUI;
        private float deltaTime = 0.0f;

        private void Update() {
            if (Time.deltaTime <= 0f) return;

            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

            textUI.text = $"{1.0f / deltaTime:0.} FPS";
        }
    }
}