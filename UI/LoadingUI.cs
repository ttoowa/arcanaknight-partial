using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class LoadingUI : MonoBehaviour {
        public static LoadingUI Instance { get; private set; }

        public GameObject panel;

        private int stack = 0;

        public static void Push() {
            ++Instance.stack;
            FadeStack.Push();

            Instance.StackChanged();
        }

        public static void Pop() {
            --Instance.stack;
            FadeStack.Pop();

            Instance.StackChanged();
        }

        private void Awake() {
            Instance = this;

            StackChanged();
        }

        private void StackChanged() {
            panel.SetActive(stack > 0);
        }
    }
}