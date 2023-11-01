using System;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class ObjectDestroyer : MonoBehaviour, IPauseable {
        public float delaySeconds = 1f;

        private float elapsedSeconds;

        private void Update() {
            elapsedSeconds += Time.deltaTime;

            if (elapsedSeconds >= delaySeconds)
                Destroy(gameObject);
        }
    }
}