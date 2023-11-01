using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient.Game {
    [ExecuteInEditMode]
    public class MonsterResource : MonoBehaviour {
        public static MonsterResource Instance { get; private set; }

        public MonsterLibrary library;

        private void Awake() {
            Instance = this;

            library.Indexing();
        }

#if UNITY_EDITOR
        private void Update() {
            Instance = this;
        }
#endif
    }
}