using System;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class DialogResource : MonoBehaviour {
        public static DialogResource Instance { get; private set; }

        public Transform dialogArea;

        public GameObject confirmDialogPrefab;

        private void Awake() {
            Instance = this;
        }
    }
}