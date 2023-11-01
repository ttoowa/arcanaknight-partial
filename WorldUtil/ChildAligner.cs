using System;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    [ExecuteInEditMode]
    public class ChildAligner : MonoBehaviour {
        public Vector2 offset;

        private void Update() {
            for (int i = 0; i < transform.childCount; i++) {
                Transform child = transform.GetChild(i);
                child.localPosition = offset * i;
            }
        }
    }
}