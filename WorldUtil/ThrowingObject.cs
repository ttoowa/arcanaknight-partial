using System;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    /// <summary>
    ///     한 방향으로 지속해서 이동하는 오브젝트
    /// </summary>
    public class ThrowingObject : MonoBehaviour, IPauseable {
        public bool syncAngle;
        public float speed;

        public Vector3 Normal {
            get => normal;
            set => normal = value.normalized;
        }

        private Vector3 normal = new(1f, 0f, 0f);

#if UNITY_EDITOR
        private float virtualPosition;
        private float previewElapsedTime;
#endif

        private void FixedUpdate() {
            transform.localPosition += normal * (speed * Time.deltaTime);

            if (syncAngle) {
                float angleY = Mathf.Atan2(normal.x, normal.z) * Mathf.Rad2Deg;
                transform.localRotation = Quaternion.Euler(0f, angleY, 0f);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos() {
            virtualPosition += speed * Time.deltaTime;
            previewElapsedTime += Time.deltaTime;

            // 이동하는 원을 그려 Preview 제공

            Color gizmoColor = Color.red;
            gizmoColor.a = 0.3f;
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(transform.position, 0.5f);
            Gizmos.DrawWireSphere(transform.position + new Vector3(virtualPosition, 0f, 0f), 0.5f);

            if (previewElapsedTime > 1f) {
                previewElapsedTime = 0;
                virtualPosition = 0;
            }
        }
#endif
    }
}