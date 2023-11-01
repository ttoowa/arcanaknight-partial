using System;
using TMPro;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    [RequireComponent(typeof(CanvasGroup))]
    public class ToastMessageUI : MonoBehaviour {
        private const float DelaySec = 1f;
        private const float FadeSec = 1f;

        public TextMeshProUGUI text;

        private CanvasGroup canvasGroup;

        private float elapsedTime;


        public void SetText(string text) {
            this.text.text = text;
        }

        private void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Update() {
            elapsedTime += Time.deltaTime;

            float easePower = 0.5f;
            float size = (Easing.EaseOutElastic(Mathf.Min(1f, elapsedTime)) - 1f) * easePower + 1f;
            transform.localScale = new Vector3(size, size, 1f);

            float alpha = Mathf.Lerp(1f, 0f, Mathf.Clamp01((elapsedTime - DelaySec) / FadeSec));
            canvasGroup.alpha = alpha;

            if (alpha <= 0f)
                Destroy(gameObject);
        }
    }
}