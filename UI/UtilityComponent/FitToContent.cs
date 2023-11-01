using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient {
    [RequireComponent(typeof(RectTransform))]
    [ExecuteInEditMode]
    public class FitToContent : MonoBehaviour {
        private RectTransform rectTransform;
        private RectTransform parent;

        public bool fitWidth;
        public bool fitHeight;

        public Vector2 padding;
        public Vector2 minSize;

        public bool limitToParent;

        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
            parent = rectTransform.parent?.GetComponent<RectTransform>();
        }

        private void Start() {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif

            FitUIManager.Instance.AddContent(this);
        }

        private void OnDestroy() {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif

            FitUIManager.Instance?.RemoveContent(this);
        }

#if UNITY_EDITOR
        private void Update() {
            if (!Application.isPlaying) {
                if (rectTransform == null)
                    rectTransform = GetComponent<RectTransform>();

                if (parent == null)
                    parent = rectTransform.parent?.GetComponent<RectTransform>();

                Fit();
            }
        }
#endif
        public void Fit() {
            if (rectTransform.childCount <= 0) return;

            RectTransform[] childTrsfs = rectTransform.GetChilds() //
                .Where(x => x.Get<IgnoreMeasure>() == null && x.activeSelf) //
                .Select(x => {
                    return x.Get<RectTransform>();
                }).ToArray();

            if (childTrsfs.Length <= 0) return;

            if (fitHeight) {
                try {
                    VerticalLayoutGroup verticalLayoutGroup = rectTransform.GetComponent<VerticalLayoutGroup>();
                    if (verticalLayoutGroup != null) {
                        verticalLayoutGroup.CalculateLayoutInputVertical();
                        verticalLayoutGroup.SetLayoutVertical();
                    }
                } catch {
                }
            }

            if (fitWidth) {
                try {
                    HorizontalLayoutGroup horizontalLayoutGroup = rectTransform.GetComponent<HorizontalLayoutGroup>();
                    if (horizontalLayoutGroup != null) {
                        horizontalLayoutGroup.CalculateLayoutInputHorizontal();
                        horizontalLayoutGroup.SetLayoutHorizontal();
                    }
                } catch {
                }
            }

            RectTransform firstChild = childTrsfs[0].GetComponent<RectTransform>();
            Rect bounds = GetCanvasBounds(firstChild);
            for (int i = 1; i < childTrsfs.Length; i++) {
                RectTransform child = childTrsfs[i].GetComponent<RectTransform>();
                Rect compareBounds = GetCanvasBounds(child);

                bounds = Rect.MinMaxRect( //
                    Mathf.Min(bounds.xMin, compareBounds.xMin), //
                    Mathf.Min(bounds.yMin, compareBounds.yMin), //
                    Mathf.Max(bounds.xMax, compareBounds.xMax), // 
                    Mathf.Max(bounds.yMax, compareBounds.yMax));
            }

            Rect GetCanvasBounds(RectTransform element) {
                return Rect.MinMaxRect(element.localPosition.x - element.rect.width * element.pivot.x, //
                    element.localPosition.y - element.rect.height * element.pivot.y, //
                    element.localPosition.x + element.rect.width * (1 - element.pivot.x), //
                    element.localPosition.y + element.rect.height * (1 - element.pivot.y));
            }

            if (limitToParent) {
                bounds.width = Mathf.Min(parent.rect.width, bounds.width);
                bounds.height = Mathf.Min(parent.rect.height, bounds.height);
            }

            bounds.width = Mathf.Max(bounds.width, minSize.x);
            bounds.height = Mathf.Max(bounds.height, minSize.y);

            if (fitWidth)
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, bounds.width + padding.x);

            if (fitHeight)
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, bounds.height + padding.y);
        }
    }
}