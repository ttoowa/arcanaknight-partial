using System;
using TMPro;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AssetText : MonoBehaviour {
        private float goldAssetPosY = 0f;
        private float goldAssetOffsetY;

        public int Value { get; private set; }
        private RectTransform rectTransform;
        private TextMeshProUGUI tmpText;

        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
            tmpText = GetComponent<TextMeshProUGUI>();

            goldAssetPosY = rectTransform.anchoredPosition.y;
        }

        private void FixedUpdate() {
            goldAssetOffsetY += (0f - goldAssetOffsetY) * 0.1f;
            Vector2 pos = rectTransform.anchoredPosition;
            pos.y = goldAssetPosY + goldAssetOffsetY;
            rectTransform.anchoredPosition = pos;
        }

        public void SetValue(int newValue) {
            if (newValue > Value)
                goldAssetOffsetY = 10f;
            else if (newValue < Value)
                goldAssetOffsetY = -10f;

            Value = newValue;
            tmpText.text = newValue.ToString();
        }
    }
}