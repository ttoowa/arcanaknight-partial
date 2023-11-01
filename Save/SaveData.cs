using System;
using System.Collections;
using System.Threading.Tasks;
using ArcaneSurvivorsClient.Game;
using ArcaneSurvivorsClient.Service;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class SaveData : MonoBehaviour {
        public static SaveData Instance { get; private set; }

        private const float SaveInterval = 3f;

        private bool isLoaded;
        private bool needSave;
        private float elapsedTime;


        public static void MarkAsDirty(bool saveImmediately = false) {
            Instance.needSave = true;

            if (saveImmediately)
                Instance.elapsedTime = SaveInterval - 0.01f;
        }

        public static void Save() {
            JObject jSlot = new();
            jSlot["GlobalStatus"] = GlobalStatus.Instance.ToJObject();
            jSlot["UpgradableStat"] = UpgradableStatManager.Instance.ToJObject();
            jSlot["GameModeState"] = GameModeManager.Instance.ToJObject();
            jSlot["CharacterSetting"] = PlayableCharacterManager.Instance.ToJObject();
            jSlot["GameSettings"] = GameSettings.Instance.ToJObject();
            jSlot["PlayRecord"] = PlayRecord.Instance.ToJObject();
            jSlot["TermsAndConditions"] = TermsAndConditions.Instance.ToJObject();

#if UNITY_EDITOR
            PlayerPrefs.SetString("SaveData", jSlot.ToString());
            PlayerPrefs.Save();
#else
            MobilePlatformManager.SaveGame(jSlot.ToString());
#endif
        }

        public static async Task Load() {
#if UNITY_EDITOR
            string rawData = PlayerPrefs.GetString("SaveData", null);
#else
            Task<string> loadTask = MobilePlatformManager.LoadGame();

            while (!loadTask.IsCompleted) {
                await Task.Delay(100);
            }

            string rawData = loadTask.Result;
#endif

            Instance.isLoaded = true;

            if (string.IsNullOrWhiteSpace(rawData))
                return;

            JObject jSlot = JObject.Parse(rawData);

            GlobalStatus.Instance.LoadFromJObject(jSlot.TryGetValue<JObject>("GlobalStatus", null));
            UpgradableStatManager.Instance.LoadFromJObject(jSlot.TryGetValue<JObject>("UpgradableStat", null));
            GameModeManager.Instance.LoadFromJObject(jSlot.TryGetValue<JObject>("GameModeState", null));
            PlayableCharacterManager.Instance.LoadFromJObject(jSlot.TryGetValue<JObject>("CharacterSetting", null));
            GameSettings.Instance.LoadFromJObject(jSlot.TryGetValue<JObject>("GameSettings", null));
            PlayRecord.Instance.LoadFromJObject(jSlot.TryGetValue<JObject>("PlayRecord", null));
            TermsAndConditions.Instance.LoadFromJObject(jSlot.TryGetValue<JObject>("TermsAndConditions", null));
        }

        public static void SaveInteger(string key, int value) {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }

        public static int LoadInteger(string key, int defaultValue) {
            if (PlayerPrefs.HasKey(key))
                return PlayerPrefs.GetInt(key, defaultValue);
            else
                return defaultValue;
        }

        private void Awake() {
            Instance = this;
        }

        private void Update() {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= SaveInterval) {
                elapsedTime = 0f;
                if (needSave && isLoaded) {
                    Save();
                    needSave = false;
                }
            }
        }
    }
}