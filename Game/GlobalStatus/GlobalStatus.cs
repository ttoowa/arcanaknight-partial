using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class GlobalStatus : MonoBehaviour {
        public static GlobalStatus Instance { get; private set; }

        public RuntimeValue<int> SP = new();

        private void Awake() {
            Instance = this;

            SP.ValueChanged += (value) => {
                VariableTextStorage.SetValue("SP", value.ToString());
                SaveData.MarkAsDirty();
            };
            SP.InvokeValueChanged();
        }

        public JObject ToJObject() {
            JObject jGlobalStatus = new();

            jGlobalStatus["SP"] = SP.Value;

            return jGlobalStatus;
        }

        public void LoadFromJObject(JObject jGlobalStatus) {
            if (jGlobalStatus == null) return;

            try {
                int sp = SP.Value;
                jGlobalStatus.TryGetValue("SP", ref sp);
                SP.Value = sp;
            } catch (Exception ex) {
                LogBuilder.Log(LogType.Error, "GlobalStatus.LoadFromJObject", $"Failed to load GlobalStatus.",
                    new[] { new LogElement("Exception", ex.ToString()) });
            }
        }

        public void AddSP(int value) {
            SP.Value += value;
        }

        public bool SubtractSP(int value) {
            if (SP.Value < value) return false;
            SP.Value -= value;
            return true;
        }
    }
}