using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class GameModeManager : MonoBehaviour {
        public static GameModeManager Instance { get; private set; }

        public GameModeLibrary modeLibrary;

        private void Awake() {
            Instance = this;
            modeLibrary.Init();
        }

        private void Start() {
        }

        public JObject ToJObject() {
            JObject jActManagerState = new();

            JArray jModes = new();
            jActManagerState.Add("Modes", jModes);

            foreach (GameMode mode in modeLibrary.dataObjects) {
                jModes.Add(mode.ToJObject());
            }

            return jActManagerState;
        }

        public void LoadFromJObject(JObject jActManagerState) {
            if (jActManagerState == null) return;

            try {
                JArray jModes = jActManagerState.TryGetValue<JArray>("Modes", null);

                foreach (JObject jMode in jModes) {
                    string modeName = jMode.TryGetValue<string>("name", null);
                    if (string.IsNullOrEmpty(modeName)) continue;

                    GameMode mode = modeLibrary.GetData(modeName);
                    if (mode == null) continue;

                    mode.LoadFromJObject(jMode);
                }
            } catch (Exception ex) {
                LogBuilder.Log(LogType.Error, "StandardGame.LoadFromJObject", $"Failed to load StandardGame.",
                    new[] { new LogElement("Exception", ex.ToString()) });
            }
        }
    }
}