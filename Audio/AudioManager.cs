using System;
using UnityEngine;
using UnityEngine.Audio;

namespace ArcaneSurvivorsClient {
    public class AudioManager : MonoBehaviour {
        public static AudioManager Instance { get; private set; }

        private RangeF BgmVolumeRange = new(-40f, -9f);
        private RangeF SfxVolumeRange = new(-40f, 0f);

        public float BgmVolume {
            get => bgmVolume;
            set {
                bgmVolume = value;
                bgmMixer.SetFloat("MasterVolume", BgmVolumeRange.Sample(value));
            }
        }

        public float SfxVolume {
            get => sfxVolume;
            set {
                sfxVolume = value;
                sfxMixer.SetFloat("MasterVolume", SfxVolumeRange.Sample(value));
            }
        }

        public bool EnableVibration { get; set; }

        private float bgmVolume;
        private float sfxVolume;

        public Transform bgmArea;
        public Transform sfxArea;

        public AudioMixer bgmMixer;
        public AudioMixer sfxMixer;

        private void Awake() {
            Instance = this;
        }
    }
}