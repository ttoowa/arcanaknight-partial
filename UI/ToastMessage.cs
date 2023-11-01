using System;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class ToastMessage : MonoBehaviour {
        public static ToastMessage Instance { get; private set; }

        public RectTransform toastArea;
        public GameObject toastMessagePrefab;

        public static void Show(string message) {
            ToastMessageUI toastMessage = Instance.toastMessagePrefab.Instantiate(Instance.toastArea)
                .GetComponent<ToastMessageUI>();
            toastMessage.SetText(message);

            SfxPlayer.Play("toast");
        }

        private void Awake() {
            Instance = this;
        }
    }
}