using UnityEngine;

namespace ArcaneSurvivorsClient {
    [RequireComponent(typeof(RectTransform))]
    [ExecuteInEditMode]
    public class MaxSize : MonoBehaviour {
        public Vector2 margin;
        public Vector2 maxSize;
        public bool useMaxWidth;
        public bool useMaxHeight;

        private RectTransform rectTransform;
        private RectTransform parentRectTransform;


        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
            parentRectTransform = transform.parent.GetComponent<RectTransform>();
        }

        private void Update() {
#if UNITY_EDITOR
            if (!Application.isPlaying) {
                if (rectTransform == null)
                    rectTransform = GetComponent<RectTransform>();
                if (parentRectTransform == null)
                    parentRectTransform = transform.parent.GetComponent<RectTransform>();
            }
#endif

            Vector2 preferredSize = new(parentRectTransform.rect.width - margin.x,
                parentRectTransform.rect.height - margin.y);

            if (useMaxWidth) {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                    Mathf.Min(preferredSize.x, maxSize.x));
            }

            if (useMaxHeight) {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                    Mathf.Min(preferredSize.y, maxSize.y));
            }
        }
    }
}