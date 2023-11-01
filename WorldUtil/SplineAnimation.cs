using System;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class SplineAnimation : MonoBehaviour, IPauseable {
        public bool IsCompleted => elapsedSeconds >= duration;
        
        [Tooltip("Need 4 positions")]
        public Vector3[] positions = new Vector3[4];

        public EasingPreset easePreset = EasingPreset.Linear;
        public float duration = 1f;
        public bool destroyOnComplete;
        public float destroyDelay;
        public GameObject[] destroyOnCompleteObjects;
        public GameObject[] instantiateOnCompleteObjects;
        
        private float elapsedSeconds = 0f;
        private bool isReserveDestroy;
        private bool isCompleted;

        public event Action Completed;

        private void Update() {
            if (IsCompleted) {
                if (!isCompleted) {
                    isCompleted = true;
                    Completed?.Invoke();

                    foreach (var obj in destroyOnCompleteObjects)
                        Destroy(obj);

                    foreach (var obj in instantiateOnCompleteObjects)
                        obj.InstantiateFX(transform.position, false);
                }
                if (destroyOnComplete && !isReserveDestroy) {
                    isReserveDestroy = true;

                    Destroy(gameObject, destroyDelay);
                }
            }

            elapsedSeconds += Time.deltaTime;

            float normalizedTime = Mathf.Clamp01(elapsedSeconds / duration);
            float easingTime = Easing.EasePreset(normalizedTime, easePreset);

            transform.localPosition =
                GSpline3D.Bezier3(easingTime, positions[0], positions[1], positions[2], positions[3]);
        }
    }
}