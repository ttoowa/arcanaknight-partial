using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class FadeStack : MonoBehaviour {
        public static FadeStack Instance { get; private set; }

        public GameObject fadeObject;

        private int stack = 0;

        public static void Push() {
            ++Instance.stack;

            Instance.StackChanged();
        }

        public static void Pop() {
            --Instance.stack;

            Instance.StackChanged();
        }

        private void Awake() {
            Instance = this;

            StackChanged();
        }

        private void StackChanged() {
            fadeObject.SetActive(stack > 0);
        }
    }
}