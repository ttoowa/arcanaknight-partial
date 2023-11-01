using System;
using System.Collections;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class LifecycleState : MonoBehaviour {
        public static bool IsInitialized { get; private set; }

        private void Start() {
            StartCoroutine(StartRoutine());
        }

        private IEnumerator StartRoutine() {
            yield return null;

            IsInitialized = true;
        }
    }
}