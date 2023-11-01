using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient {
    public class BgmPlayer : MonoBehaviour {
        public static BgmPlayer Instance { get; private set; }

        public AudioMixerGroup mixerGroup;
        public AudioClip[] clips;

        public AudioPlayer CurrentPlayer { get; private set; }
        public string CurrentKey { get; private set; }

        private readonly List<AudioPlayer> playerList = new();
        private readonly Dictionary<string, AudioPlayer> playerDict = new();
        private readonly Dictionary<string, AudioClip> clipDict = new();

        public float fadeDuration = 2f;

        private void Awake() {
            Instance = this;

            Indexing();
        }

        private void Indexing() {
            foreach (AudioClip clip in clips) {
                if (clip == null) continue;
                clipDict.Add(clip.name, clip);
            }
        }

        public static void Play(string key) {
            Instance._Play(key);
        }

        private void _Play(string key) {
            if (CurrentKey == key) return;
            CurrentKey = key;

            if (playerDict.ContainsKey(key))
                CurrentPlayer = playerDict[key];
            else {
                if (!clipDict.ContainsKey(key)) {
                    LogBuilder.Log(LogType.Warning, nameof(BgmPlayer), $"Not found '{key}' in elements.");
                    return;
                }

                CurrentPlayer = new AudioPlayer(clipDict[key], AudioManager.Instance.bgmArea, mixerGroup);
                CurrentPlayer.IsLoop = true;
                CurrentPlayer.Volume = 0f;

                playerList.Add(CurrentPlayer);
                playerDict.Add(key, CurrentPlayer);
            }

            if (!CurrentPlayer.IsPlaying) {
                CurrentPlayer.Volume = 0f;
                CurrentPlayer.Time = 0f;
                CurrentPlayer.Play();
            }
        }

        private void Update() {
            foreach (AudioPlayer player in playerList) {
                bool isCurrent = player == CurrentPlayer;

                if (isCurrent && player.Volume < 1f)
                    player.Volume = Mathf.Clamp01(player.Volume + Time.deltaTime / fadeDuration);
                else if (!isCurrent && player.Volume > 0f) {
                    player.Volume = Mathf.Clamp01(player.Volume - Time.deltaTime / fadeDuration);

                    if (player.Volume <= 0f && player.IsPlaying)
                        player.Stop();
                }
            }
        }
    }
}