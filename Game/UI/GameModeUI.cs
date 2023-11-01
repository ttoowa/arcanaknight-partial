using System;
using ArcaneSurvivorsClient.Locale;
using TMPro;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class GameModeUI : MonoBehaviour {
        [Serializable]
        public class StandardGame {
        }

        public static GameModeUI Instance { get; private set; }

        public StandardGame standardGame;

        private void Awake() {
            Instance = this;
        }
    }
}