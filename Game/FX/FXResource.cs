using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient.Game {
    public class FXResource : MonoBehaviour, ISingletone {
        public static FXResource Instance { get; private set; }

        public Transform FXArea;

        public GameObject playerDamageFXPrefab;
        public GameObject monsterDamageFXPrefab;

        public GameObject pickupFieldItemFXPrefab;

        public GameObject hitFXPrefab;

        public GameObject weaponLvUpFXPrefab;

        public GameObject weaponMergeTrailFXPrefab;
        public GameObject weaponMergeFXPrefab;

        private void Awake() {
            Instance = this;
        }
    }
}