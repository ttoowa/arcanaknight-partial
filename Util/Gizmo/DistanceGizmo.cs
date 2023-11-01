using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ArcaneSurvivorsClient.Gizmo {
    public class DistanceGizmo : MonoBehaviour {
#if UNITY_EDITOR
        public float[] radiuses;

        private void OnDrawGizmos() {
            Color color = Color.grey;
            color.a = 0.5f;
            Gizmos.color = color;

            foreach (float radius in radiuses) {
                Gizmos.DrawWireSphere(transform.position, radius);
                Handles.Label(transform.position + new Vector3(-radius, 0f, 0f), $"{radius}");
            }
        }
#endif
    }
}