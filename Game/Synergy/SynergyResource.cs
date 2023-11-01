using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient.Game {
    public class SynergyResource : MonoBehaviour {
        public static SynergyResource Instance { get; private set; }

        public SynergyLibrary library;

        public GameObject synergyBadgePrefab;
        public GameObject synergyBundlePrefab;
        public GameObject synergyBuffDescriptionPrefab;

        private Dictionary<SynergyType, Synergy> synergyDict;

        private void Awake() {
            Instance = this;

            library.Indexing();
        }
    }
}