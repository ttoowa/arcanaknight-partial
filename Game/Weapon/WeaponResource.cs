using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ArcaneSurvivorsClient.Game {
    public class WeaponResource : MonoBehaviour, ISingletone {
        public static WeaponResource Instance { get; private set; }

        public WeaponLibrary library;
        public Material[] weaponMaterials;

        private void Awake() {
            Instance = this;

            library.Indexing();
        }
    }
}