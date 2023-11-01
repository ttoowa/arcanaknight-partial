using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class RuntimeAudioElement {
        public bool CanPlay => delayLeftTime <= 0f;
        public AudioClip Clip => model.clip;

        public AudioElement model;
        private float delayLeftTime;

        public RuntimeAudioElement(AudioElement model) {
            this.model = model;
        }

        public void UpdateLeftDelay(float deltaTime) {
            delayLeftTime = Mathf.Max(delayLeftTime - deltaTime, 0f);
        }

        public void SetOverlapDelay() {
            delayLeftTime = model.overlapDelay;
        }
    }
}