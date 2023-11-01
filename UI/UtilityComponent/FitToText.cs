using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(TextMeshProUGUI))]
    [ExecuteInEditMode]
    public class FitToText : MonoBehaviour {
        private RectTransform rectTransform;
        private TextMeshProUGUI text;

        public Vector2 margin;

        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
            text = GetComponent<TextMeshProUGUI>();
        }

        private void Start() {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif

            FitUIManager.Instance.AddText(this);
        }

        private void OnDestroy() {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif

            FitUIManager.Instance?.RemoveText(this);
        }

#if UNITY_EDITOR
        private void Update() {
            if (!Application.isPlaying) {
                if (rectTransform == null)
                    rectTransform = GetComponent<RectTransform>();

                if (text == null)
                    text = GetComponent<TextMeshProUGUI>();

                Fit();
            }
        }
#endif

        public void Fit() {
            // Handle text size
            if (text != null) {
                Vector2 size = (Vector2)text.bounds.size + margin;

                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
            }
        }
    }
}