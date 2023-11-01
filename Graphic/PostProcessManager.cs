using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ArcaneSurvivorsClient {
    public class PostProcessManager : MonoBehaviour {
        public static PostProcessManager Instance { get; private set; }

        [SerializeField]
        private Volume volume;

        private ColorAdjustments colorAdjustments;

        private void Awake() {
            Instance = this;

            volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        }

        public void SetSaturate(float weight) {
            if (colorAdjustments == null) return;
            colorAdjustments.saturation.value = weight;
        }
    }
}