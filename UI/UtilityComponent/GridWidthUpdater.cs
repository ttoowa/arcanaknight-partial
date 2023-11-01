using UnityEngine;

namespace ArcaneSurvivorsClient {
    [RequireComponent(typeof(RectTransform))]
    [ExecuteInEditMode]
    public class GridWidthUpdater : MonoBehaviour {
        public float unitSize = 220f;
        public float margin = 30f;
        public int minColumns = 2;

        private RectTransform rectTransform;
        public RectTransform canvas;


        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update() {
            if (canvas == null) return;
#if UNITY_EDITOR
            if (!Application.isPlaying) {
                if (rectTransform == null)
                    rectTransform = GetComponent<RectTransform>();
            }
#endif

            float a = canvas.sizeDelta.x - margin * 2f;
            int gridColumns = Mathf.Max(minColumns, Mathf.FloorToInt(a / unitSize));

            float width = gridColumns * unitSize;

            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }
    }
}