using System;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient {
    [ExecuteInEditMode]
    public class LayoutGroupUpdater : MonoBehaviour {
        private HorizontalLayoutGroup horizontalLayoutGroup;
        private VerticalLayoutGroup verticalLayoutGroup;

        private void Awake() {
            horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
            verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        }

        private void OnEnable() {
            Update();
        }

        private void Update() {
#if UNITY_EDITOR
            if (!Application.isPlaying) {
                if (horizontalLayoutGroup == null)
                    horizontalLayoutGroup = GetComponent<HorizontalLayoutGroup>();
                if (verticalLayoutGroup == null)
                    verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
            }
#endif

            if (horizontalLayoutGroup != null) {
                horizontalLayoutGroup.enabled = false;
                horizontalLayoutGroup.enabled = true;
            }

            if (verticalLayoutGroup != null) {
                verticalLayoutGroup.enabled = false;
                verticalLayoutGroup.enabled = true;
            }
        }
    }
}