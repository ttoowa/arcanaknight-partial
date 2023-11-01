using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class PlayRecord : MonoBehaviour {
        public static PlayRecord Instance { get; private set; }

        public RuntimeValue<int> startCount = new();
        public RuntimeValue<int> playCount = new();
        public RuntimeValue<int> clearCount = new();
        public RuntimeValue<int> gameOverCount = new();

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            IRuntimeValue[] values = new IRuntimeValue[] {
                startCount,
                playCount,
                clearCount,
                gameOverCount
            };

            foreach (IRuntimeValue value in values) {
                value.ValueChangedSimple += () => {
                    SaveData.MarkAsDirty();
                };
                value.InvokeValueChanged();
            }

            playCount.ValueChanged += (value) => {
                if (!GameManager.Instance.IsPlayedOnSession) return;

                if (value >= 2)
                    MobilePlatformManager.FloatReview(true);
            };
        }

        public JObject ToJObject() {
            JObject jSaveData = new();

            jSaveData["startCount"] = startCount.Value;
            jSaveData["playCount"] = playCount.Value;
            jSaveData["clearCount"] = clearCount.Value;
            jSaveData["gameOverCount"] = gameOverCount.Value;

            return jSaveData;
        }

        public void LoadFromJObject(JObject jSaveData) {
            if (jSaveData == null)
                return;

            try {
                startCount.Value = jSaveData.TryGetValue<int>("startCount", 0);
                playCount.Value = jSaveData.TryGetValue<int>("playCount", 0);
                clearCount.Value = jSaveData.TryGetValue<int>("clearCount", 0);
                gameOverCount.Value = jSaveData.TryGetValue<int>("gameOverCount", 0);
            } catch (Exception ex) {
                LogBuilder.Log(LogType.Error, "StandardGame.SaveData.LoadFromJObject",
                    $"Failed to load StandardGame.SaveData.", new[] { new LogElement("Exception", ex.ToString()) });
            }
        }
    }
}