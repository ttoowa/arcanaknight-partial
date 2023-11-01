using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Service {
    public class TermsAndConditionsUI : MonoBehaviour {
        public static TermsAndConditionsUI Instance { get; private set; }

        public GameObject panel;

        public Checkbox checkbox_IsAcceptedTermsAndConditions;
        public Checkbox checkbox_IsAcceptedCollectionOfPersonalInfo;
        public Checkbox checkbox_IsAcceptedDayPushAlarm;
        public Checkbox checkbox_IsAcceptedNightPushAlarm;

        public Button detailButton;

        public Button confirmButton;

        private void Awake() {
            Instance = this;

            SetVisible(false, false);
        }

        private void Start() {
            TermsAndConditions.Instance.isAcceptedTermsAndConditions.ValueChanged += (bool value) => {
                checkbox_IsAcceptedTermsAndConditions.SetChecked(value, false);
                OnStateChanged();
            };
            checkbox_IsAcceptedTermsAndConditions.CheckStateChanged += (bool isChecked) => {
                TermsAndConditions.Instance.isAcceptedTermsAndConditions.Value = isChecked;
            };

            TermsAndConditions.Instance.isAcceptedCollectionOfPersonalInfo.ValueChanged += (bool value) => {
                checkbox_IsAcceptedCollectionOfPersonalInfo.SetChecked(value, false);
                OnStateChanged();
            };
            checkbox_IsAcceptedCollectionOfPersonalInfo.CheckStateChanged += (bool isChecked) => {
                TermsAndConditions.Instance.isAcceptedCollectionOfPersonalInfo.Value = isChecked;
            };

            TermsAndConditions.Instance.isAcceptedDayPushAlarm.ValueChanged += (bool value) => {
                checkbox_IsAcceptedDayPushAlarm.SetChecked(value, false);
                OnStateChanged();
            };
            checkbox_IsAcceptedDayPushAlarm.CheckStateChanged += (bool isChecked) => {
                TermsAndConditions.Instance.isAcceptedDayPushAlarm.Value = isChecked;
            };

            TermsAndConditions.Instance.isAcceptedNightPushAlarm.ValueChanged += (bool value) => {
                checkbox_IsAcceptedNightPushAlarm.SetChecked(value, false);
                OnStateChanged();
            };
            checkbox_IsAcceptedNightPushAlarm.CheckStateChanged += (bool isChecked) => {
                TermsAndConditions.Instance.isAcceptedNightPushAlarm.Value = isChecked;
            };

            detailButton.onClick.AddListener(() => {
                Application.OpenURL("[URL]");
            });

            confirmButton.onClick.AddListener(() => {
                TermsAndConditions.Instance.isCollectedAnswers.Value = true;
                SetVisible(false);
            });

            OnStateChanged();
        }

        public void SetVisible(bool visible, bool withFade = true) {
            panel.SetActive(visible);

            if (withFade) {
                if (visible)
                    FadeStack.Push();
                else
                    FadeStack.Pop();
            }
        }

        private void OnStateChanged() {
            confirmButton.interactable = TermsAndConditions.Instance.IsAcceptedRequiredItems;
        }
    }
}