using System;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class PlayableCharacterResource : MonoBehaviour {
        public static PlayableCharacterResource Instance { get; private set; }

        public PlayableCharacterLibrary library;

        private void Awake() {
            Instance = this;

            library.Indexing();
        }
    }
}