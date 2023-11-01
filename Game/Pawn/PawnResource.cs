using System;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class PawnResource : MonoBehaviour, ISingletone {
        public static PawnResource Instance { get; private set; }

        public GameObject playerPawnPrefab;
        public GameObject dummyPawnPrefab;

        private void Awake() {
            Instance = this;
        }
    }
}