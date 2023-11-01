using System;
using ArcaneSurvivorsClient.Game;
using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient {
    public class GlobalUI : MonoBehaviour, ISingletone {
        public static GlobalUI Instance { get; private set; }

        public Canvas canvas_UI;
        public RectTransform canvasTrsf_UI;

        public RectTransform worldUIArea;
        public RectTransform worldFXArea;

        public RectTransform topFXArea;

        private void Awake() {
            Instance = this;
        }
    }
}