using System;
using TMPro;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class BlinkTextAnimation : MonoBehaviour {
        public float speed = 1f;

        [Range(0f, 1f)]
        public float minAlpha = 0.3f;

        private float elapsedSeconds;
        private TextMeshProUGUI text;

        private void Start() {
            text = GetComponent<TextMeshProUGUI>();
        }

        private void Update() {
            elapsedSeconds += Time.deltaTime;

            float alpha = Mathf.Sin(elapsedSeconds * speed) * 0.5f + 0.5f;
            alpha = alpha * (1f - minAlpha) + minAlpha;

            text.alpha = alpha;
        }
    }
}