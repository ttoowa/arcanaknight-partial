using System;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class WaveAnimation : MonoBehaviour {
        public Vector3 distance = new(0f, 1f, 0f);
        public float speed = 1f;

        private Vector3 originalPosition;

        private float elapsedTime;

        private void Awake() {
            originalPosition = transform.localPosition;
        }

        private void Update() {
            elapsedTime += Time.deltaTime * speed;
            Vector3 offset = Mathf.Sin(elapsedTime) * distance;
            transform.localPosition = originalPosition + offset;
        }

        private void OnDrawGizmos() {
            //Matrix4x4 tempMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(-distance, distance);
        }
    }
}