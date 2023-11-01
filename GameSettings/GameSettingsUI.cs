using System;
using ArcaneSurvivorsClient.Locale;
using ArcaneSurvivorsClient.Service;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient {
    public class GameSettingsUI : MonoBehaviour {
        private interface ISettingTab {
            public void Init();
        }

        [Serializable]
        public class Language : ISettingTab {
            public ListView languageListView;

            public void Init() {
                languageListView.ClearItems();
                foreach (string language in LocaleManager.Instance.Languages) {
                    LanguageElementUI element = languageListView.AddItem(language) as LanguageElementUI;
                    element.languageText.SpecificLanguage = language;

                    element.SelectedStateChanged += (bool isSelected) => {
                        if (!isSelected) return;
                        GameSettings.Instance.language.language.Value = language;
                    };
                }

                GameSettings.Instance.language.language.ValueChanged += (string value) => {
                    if (value == null) return;

                    languageListView.SelectItem(value, false);
                };
                GameSettings.Instance.language.language.InvokeValueChanged();
            }
        }

        [Serializable]
        public class Alert : ISettingTab {
            public Checkbox isAcceptedDayPushAlarmCheckbox;
            public Checkbox isAcceptedNightPushAlarmCheckbox;

            public void Init() {
                isAcceptedDayPushAlarmCheckbox.CheckStateChanged += (bool isChecked) => {
                    TermsAndConditions.Instance.isAcceptedDayPushAlarm.Value = isChecked;
                };
                TermsAndConditions.Instance.isAcceptedDayPushAlarm.ValueChanged += (bool value) => {
                    isAcceptedDayPushAlarmCheckbox.SetChecked(value, false);
                };
                TermsAndConditions.Instance.isAcceptedDayPushAlarm.InvokeValueChanged();

                isAcceptedNightPushAlarmCheckbox.CheckStateChanged += (bool isChecked) => {
                    TermsAndConditions.Instance.isAcceptedNightPushAlarm.Value = isChecked;
                };
                TermsAndConditions.Instance.isAcceptedNightPushAlarm.ValueChanged += (bool value) => {
                    isAcceptedNightPushAlarmCheckbox.SetChecked(value, false);
                };
                TermsAndConditions.Instance.isAcceptedNightPushAlarm.InvokeValueChanged();
            }
        }

        [Serializable]
        public class Account : ISettingTab {
            public Button resetDataButton;

            public void Init() {
            }
        }

        [Serializable]
        public class Graphic : ISettingTab {
            public RadioButtonGroup enableVsyncSelector;
            public RadioButtonGroup targetFrameRateSelector;
            public Slider brightnessSlider;

            public void Init() {
                enableVsyncSelector.SelectedStateChanged += (RadioButton radioButton, bool isSelected) => {
                    if (!isSelected) return;
                    GameSettings.Instance.graphic.enableVsync.Value = radioButton.BoolValue;
                };
                GameSettings.Instance.graphic.enableVsync.ValueChanged += (bool value) => {
                    enableVsyncSelector.Select(value.ToString(), false);
                };
                GameSettings.Instance.graphic.enableVsync.InvokeValueChanged();

                targetFrameRateSelector.SelectedStateChanged += (RadioButton radioButton, bool isSelected) => {
                    if (!isSelected) return;
                    GameSettings.Instance.graphic.targetFrameRate.Value = radioButton.IntValue;
                };
                GameSettings.Instance.graphic.targetFrameRate.ValueChanged += (int value) => {
                    targetFrameRateSelector.Select(value.ToString(), false);
                };
                GameSettings.Instance.graphic.targetFrameRate.InvokeValueChanged();
            }
        }

        [Serializable]
        public class Audio : ISettingTab {
            public Slider bgmVolumeSlider;
            public Slider sfxVolumeSlider;
            public RadioButtonGroup vibrationSelector;

            public void Init() {
                bgmVolumeSlider.ValueChanged += (float value) => {
                    GameSettings.Instance.audio.bgmVolume.Value = value;
                };
                GameSettings.Instance.audio.bgmVolume.ValueChanged += (float value) => {
                    bgmVolumeSlider.SetValue(value, false);
                };
                GameSettings.Instance.audio.bgmVolume.InvokeValueChanged();

                sfxVolumeSlider.ValueChanged += (float value) => {
                    GameSettings.Instance.audio.sfxVolume.Value = value;
                };
                GameSettings.Instance.audio.sfxVolume.ValueChanged += (float value) => {
                    sfxVolumeSlider.SetValue(value, false);
                };
                GameSettings.Instance.audio.sfxVolume.InvokeValueChanged();

                vibrationSelector.SelectedStateChanged += (RadioButton radioButton, bool isSelected) => {
                    if (!isSelected) return;
                    GameSettings.Instance.audio.enableVibration.Value = radioButton.BoolValue;
                };
                GameSettings.Instance.audio.enableVibration.ValueChanged += (bool value) => {
                    vibrationSelector.Select(value.ToString(), false);
                };
                GameSettings.Instance.audio.enableVibration.InvokeValueChanged();
            }
        }

        public static GameSettingsUI Instance { get; private set; }

        public GameObject panel;

        public Button settingsButton;
        public Button closeButton;

        public ListView tabButtonListView;

        public Language language;
        public Alert alert;
        public Account account;
        public Graphic graphic;
        public Audio audio;

        private ISettingTab[] tabs;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            tabs = new ISettingTab[] {
                language,
                alert,
                account,
                graphic,
                audio
            };

            foreach (ISettingTab tab in tabs) {
                tab.Init();
            }

            settingsButton.onClick.AddListener(() => {
                SetVisible(true);
            });

            closeButton.onClick.AddListener(() => {
                SetVisible(false);
            });

            SetVisible(false, false);
        }

        public void SetVisible(bool visible, bool withFade = true) {
            panel.SetActive(visible);

            if (visible)
                tabButtonListView.SelectFirstItem();

            if (withFade) {
                if (visible)
                    FadeStack.Push();
                else
                    FadeStack.Pop();
            }
        }
    }
}