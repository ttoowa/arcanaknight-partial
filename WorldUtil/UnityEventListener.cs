using System;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class UnityEventListener : MonoBehaviour {
        [Serializable]
        public class UnityEvents {
            public event Action Awake;
            public event Action Start;
            public event Action OnDestroy;

            public event Action<Collision> OnCollisionEnter;
            public event Action<Collision> OnCollisionStay;
            public event Action<Collision> OnCollisionExit;
            public event Action<Collider> OnTriggerEnter;
            public event Action<Collider> OnTriggerStay;
            public event Action<Collider> OnTriggerExit;

            public void InvokeAwake() {
                Awake?.Invoke();
            }

            public void InvokeStart() {
                Start?.Invoke();
            }

            public void InvokeOnDestroy() {
                OnDestroy?.Invoke();
            }

            public void InvokeOnCollisionEnter(Collision collision) {
                OnCollisionEnter?.Invoke(collision);
            }

            public void InvokeOnCollisionStay(Collision collision) {
                OnCollisionStay?.Invoke(collision);
            }

            public void InvokeOnCollisionExit(Collision collision) {
                OnCollisionExit?.Invoke(collision);
            }

            public void InvokeOnTriggerEnter(Collider collider) {
                OnTriggerEnter?.Invoke(collider);
            }

            public void InvokeOnTriggerStay(Collider collider) {
                OnTriggerStay?.Invoke(collider);
            }

            public void InvokeOnTriggerExit(Collider collider) {
                OnTriggerExit?.Invoke(collider);
            }
        }

        public readonly UnityEvents Events = new();

        private void Awake() {
            Events.InvokeAwake();
        }

        private void Start() {
            Events.InvokeStart();
        }

        private void OnDestroy() {
            Events.InvokeOnDestroy();
        }

        private void OnCollisionEnter(Collision collision) {
            Events.InvokeOnCollisionEnter(collision);
        }

        private void OnCollisionStay(Collision collision) {
            Events.InvokeOnCollisionStay(collision);
        }

        private void OnCollisionExit(Collision collision) {
            Events.InvokeOnCollisionExit(collision);
        }

        private void OnTriggerEnter(Collider other) {
            Events.InvokeOnTriggerEnter(other);
        }

        private void OnTriggerStay(Collider other) {
            Events.InvokeOnTriggerStay(other);
        }

        private void OnTriggerExit(Collider other) {
            Events.InvokeOnTriggerExit(other);
        }
    }
}