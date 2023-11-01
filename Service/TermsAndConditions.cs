using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ArcaneSurvivorsClient.Service {
    public class TermsAndConditions : MonoBehaviour {
        public static TermsAndConditions Instance { get; private set; }

        public bool IsAcceptedRequiredItems =>
            isAcceptedTermsAndConditions.Value && isAcceptedCollectionOfPersonalInfo.Value;

        public RuntimeValue<bool> isCollectedAnswers = new();
        public RuntimeValue<bool> isAcceptedTermsAndConditions = new();
        public RuntimeValue<bool> isAcceptedCollectionOfPersonalInfo = new();
        public RuntimeValue<bool> isAcceptedDayPushAlarm = new();
        public RuntimeValue<bool> isAcceptedNightPushAlarm = new();

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            isCollectedAnswers.ValueChanged += (value) => {
                SaveData.MarkAsDirty();
            };

            isAcceptedTermsAndConditions.ValueChanged += (value) => {
                SaveData.MarkAsDirty();
            };

            isAcceptedCollectionOfPersonalInfo.ValueChanged += (value) => {
                SaveData.MarkAsDirty();
            };

            isAcceptedDayPushAlarm.ValueChanged += (value) => {
                SaveData.MarkAsDirty();
            };

            isAcceptedNightPushAlarm.ValueChanged += (value) => {
                SaveData.MarkAsDirty();
            };

            isCollectedAnswers.InvokeValueChanged();
            isAcceptedTermsAndConditions.InvokeValueChanged();
            isAcceptedCollectionOfPersonalInfo.InvokeValueChanged();
            isAcceptedDayPushAlarm.InvokeValueChanged();
            isAcceptedNightPushAlarm.InvokeValueChanged();
        }

        public JObject ToJObject() {
            JObject jTermsAndConds = new();

            jTermsAndConds["isCollectedAnswers"] = isCollectedAnswers.Value;
            jTermsAndConds["isAcceptedTermsAndConditions"] = isAcceptedTermsAndConditions.Value;
            jTermsAndConds["isAcceptedCollectionOfPersonalInfo"] = isAcceptedCollectionOfPersonalInfo.Value;
            jTermsAndConds["isAcceptedDayPushAlarm"] = isAcceptedDayPushAlarm.Value;
            jTermsAndConds["isAcceptedNightPushAlarm"] = isAcceptedNightPushAlarm.Value;

            return jTermsAndConds;
        }

        public void LoadFromJObject(JObject jTermsAndConds) {
            if (jTermsAndConds == null)
                return;

            try {
                isCollectedAnswers.Value = jTermsAndConds.TryGetValue<bool>("isCollectedAnswers", false);
                isAcceptedTermsAndConditions.Value =
                    jTermsAndConds.TryGetValue<bool>("isAcceptedTermsAndConditions", false);
                isAcceptedCollectionOfPersonalInfo.Value =
                    jTermsAndConds.TryGetValue<bool>("isAcceptedCollectionOfPersonalInfo", false);
                isAcceptedDayPushAlarm.Value = jTermsAndConds.TryGetValue<bool>("isAcceptedDayPushAlarm", false);
                isAcceptedNightPushAlarm.Value = jTermsAndConds.TryGetValue<bool>("isAcceptedNightPushAlarm", false);
            } catch (Exception ex) {
                LogBuilder.Log(LogType.Warning, "StandardGame.SaveData.LoadFromJObject",
                    $"Failed to load from JObject. {ex.Message}");
            }
        }
    }
}