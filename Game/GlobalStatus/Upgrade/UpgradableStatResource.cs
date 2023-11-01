using System;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class UpgradableStatResource : MonoBehaviour {
        public static UpgradableStatResource Instance { get; private set; }

        public UpgradableStatLibrary library;

        private void Awake() {
            Instance = this;

            library.Indexing();
        }
    }
}