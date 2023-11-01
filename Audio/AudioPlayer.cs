using UnityEngine;
using UnityEngine.Audio;

namespace ArcaneSurvivorsClient {
    public class AudioPlayer {
        public AudioSource source;

        public float Time {
            get => source.time;
            set => source.time = value;
        }

        public float Volume {
            get => source.volume;
            set => source.volume = value;
        }

        public bool IsPlaying => source.isPlaying;

        public bool IsLoop {
            get => source.loop;
            set => source.loop = value;
        }

        public float Pitch {
            get => source.pitch;
            set => source.pitch = value;
        }

        public AudioPlayer(AudioClip clip, Transform parent, AudioMixerGroup mixerGroup = null) {
            source = new GameObject("AudioSource").AddComponent<AudioSource>();
            source.clip = clip;
            source.outputAudioMixerGroup = mixerGroup;
            source.transform.SetParent(parent, false);
        }

        public void Play() {
            source.Play();
        }

        public void Stop() {
            source.Stop();
        }

        public void Pause() {
            source.Pause();
        }

        public void UnPause() {
            source.UnPause();
        }
    }
}