using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class PlayableCharacterManager : MonoBehaviour {
        public delegate void CharacterSelectedDelegate(PlayableCharacterType character);

        public delegate void AvailableCharacterChangedDelegate(HashSet<PlayableCharacterType> availableCharacterSet);

        public static PlayableCharacterManager Instance { get; private set; }

        public PlayableCharacterType SelectedCharacterType { get; private set; }

        public PlayableCharacter SelectedCharacter =>
            PlayableCharacterResource.Instance.library.GetData(SelectedCharacterType);

        public readonly HashSet<PlayableCharacterType> AvailableCharacterSet = new();

        public event CharacterSelectedDelegate CharacterSelected;
        public event AvailableCharacterChangedDelegate AvailableCharacterChanged;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            AddDefaultAvailableCharacters();
        }


        public JObject ToJObject() {
            JObject jCharacterSetting = new();
            jCharacterSetting["selectedCharacter"] = SelectedCharacterType.ToString();

            JArray jAvailableCharacters = new();
            jCharacterSetting["availableCharacters"] = jAvailableCharacters;

            foreach (PlayableCharacterType availableCharacter in AvailableCharacterSet) {
                jAvailableCharacters.Add(availableCharacter.ToString());
            }

            return jCharacterSetting;
        }

        public void LoadFromJObject(JObject jCharacterSetting) {
            if (jCharacterSetting == null) return;

            try {
                SelectedCharacterType =
                    jCharacterSetting.TryGetValue<PlayableCharacterType>("selectedCharacter", SelectedCharacterType);

                AvailableCharacterSet.Clear();
                JArray availableCharacterTypes = jCharacterSetting.TryGetValue<JArray>("availableCharacters", null);
                if (availableCharacterTypes != null) {
                    foreach (JToken availableCharacterType in availableCharacterTypes) {
                        AvailableCharacterSet.Add(availableCharacterType.ToObject<PlayableCharacterType>());
                    }
                }
            } catch (Exception ex) {
                LogBuilder.Log(LogType.Error, "PlayableCharacterManager.LoadFromJObject",
                    $"Failed to load PlayableCharacterManager.", new[] { new LogElement("Exception", ex.ToString()) });
            }
        }

        public void SelectCharacter(PlayableCharacterType characterType) {
            SelectedCharacterType = characterType;

            CharacterSelected?.Invoke(characterType);

            SaveData.MarkAsDirty();
        }

        public bool AddAvailableCharacter(PlayableCharacterType characterType) {
            if (!AvailableCharacterSet.Add(characterType)) return false;


            AvailableCharacterChanged?.Invoke(AvailableCharacterSet);

            SaveData.MarkAsDirty();

            return true;
        }

        private void AddDefaultAvailableCharacters() {
            AddAvailableCharacter(PlayableCharacterType.Karin);
        }
    }
}