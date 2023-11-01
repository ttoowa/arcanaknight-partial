using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient {
    [Serializable]
    public class AudioElement {
        public string Key => clip.name;

        public AudioClip clip;
        public float overlapDelay;

        public AudioElement(AudioClip clip) {
            this.clip = clip;
        }
    }
}