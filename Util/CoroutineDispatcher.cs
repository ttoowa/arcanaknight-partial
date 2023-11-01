using System.Collections;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class CoroutineDispatcher : MonoBehaviour, IPauseable {
        public static CoroutineDispatcher Instance { get; private set; }

        public static Coroutine Dispatch(IEnumerator coroutine) {
            return Instance.StartCoroutine(coroutine);
        }

        private void Awake() {
            Instance = this;
        }
    }
}