using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace ArcaneSurvivorsClient {
    public class SfxPlayer : MonoBehaviour {
        public static SfxPlayer Instance { get; private set; }

        public AudioMixerGroup mixerGroup;
        public AudioElement[] elements;

        private AudioSource defaultSource;
        private AudioSource[] randomPitchSources;

        private readonly Dictionary<string, RuntimeAudioElement> elementDict = new();
        private readonly List<RuntimeAudioElement> delayingElementList = new();

        private void Awake() {
            Instance = this;

            Indexing();
        }

        private void Start() {
            CreatePlayers();
        }

        private void Update() {
            UpdateDelayingElements();
        }

        private void Indexing() {
            foreach (AudioElement element in elements) {
                if (element == null || element.clip == null) continue;
                elementDict.Add(element.Key, new RuntimeAudioElement(element));
            }
        }

        public static void Play(string key, bool randomPitch = false, float volume = 1f) {
            Instance._Play(key, randomPitch, volume);
        }

        private void _Play(string key, bool randomPitch, float volume) {
            if (!elementDict.ContainsKey(key)) {
                LogBuilder.Log(LogType.Warning, nameof(SfxPlayer), $"Not found '{key}' in elements.");
                return;
            }

            AudioSource source;
            if (randomPitch)
                source = randomPitchSources[Random.Range(0, randomPitchSources.Length)];
            else
                source = defaultSource;

            RuntimeAudioElement element = elementDict[key];
            if (element.CanPlay) {
                element.SetOverlapDelay();
                source.PlayOneShot(element.Clip, volume);

                if (element.model.overlapDelay > 0f)
                    delayingElementList.Add(element);
            }
        }

        private void CreatePlayers() {
            defaultSource = CreatePlayer();

            int pitchSteps = 10;
            int halfPitchSteps = pitchSteps / 2;
            List<AudioSource> sources = new();
            for (int i = -halfPitchSteps; i < halfPitchSteps; ++i) {
                sources.Add(CreatePlayer(1f + i * 0.02f));
            }

            randomPitchSources = sources.ToArray();
        }

        private AudioSource CreatePlayer(float pitch = 1f) {
            GameObject sfxPlayer = new("SfxPlayer");
            sfxPlayer.transform.SetParent(AudioManager.Instance.sfxArea);
            AudioSource source = sfxPlayer.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = mixerGroup;
            source.pitch = pitch;

            return source;
        }

        private void UpdateDelayingElements() {
            float deltaTime = Time.deltaTime;

            List<int> removeList = new();
            for (int i = 0; i < delayingElementList.Count; ++i) {
                RuntimeAudioElement element = delayingElementList[i];
                element.UpdateLeftDelay(deltaTime);
                if (element.CanPlay)
                    removeList.Add(i);
            }

            for (int i = removeList.Count - 1; i >= 0; --i) {
                delayingElementList.RemoveAt(removeList[i]);
            }
        }
    }
}