using System;
using System.Linq;
using ArcaneSurvivorsClient.Locale;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class GameSettings : MonoBehaviour {
        private interface ISettingContent {
            public void Init();
        }

        [Serializable]
        public class Language : ISettingContent {
            public RuntimeValue<string> language = new();

            public void Init() {
                language.ValueChanged += (value) => {
                    VariableTextStorage.SetValue("Language", value);
                    LocaleManager.Instance.SetLanguage(value);
                };

                SetDevicePreferredLanguage();
            }

            private void SetDevicePreferredLanguage() {
                string deviceLanguage = "";
                switch (Application.systemLanguage) {
                    default:
                        deviceLanguage = "English";
                        break;
                    case SystemLanguage.Korean:
                        deviceLanguage = "Korean";
                        break;
                    case SystemLanguage.Thai:
                        deviceLanguage = "Thai";
                        break;
                    case SystemLanguage.Japanese:
                        deviceLanguage = "Japanese";
                        break;
                    case SystemLanguage.Indonesian:
                        deviceLanguage = "Indonesian";
                        break;
                    case SystemLanguage.Vietnamese:
                        deviceLanguage = "Vietnamese Language";
                        break;
                    case SystemLanguage.French:
                        deviceLanguage = "French";
                        break;
                    case SystemLanguage.Chinese:
                    case SystemLanguage.ChineseSimplified:
                        deviceLanguage = "Simplified Chinese";
                        break;
                    case SystemLanguage.ChineseTraditional:
                        deviceLanguage = "Traditional Chinese";
                        break;
                }

                language.Value = deviceLanguage;
            }

            public JObject ToJObject() {
                JObject jLanguage = new();

                jLanguage["language"] = language.Value;

                return jLanguage;
            }

            public void LoadFromJObject(JObject jLanguage) {
                if (jLanguage == null) return;

                try {
                    language.Value = jLanguage.TryGetValue("language", language.Value);
                } catch (Exception ex) {
                    LogBuilder.Log(LogType.Error, "Language.LoadFromJObject", $"Failed to load Language.",
                        new[] { new LogElement("Exception", ex.ToString()) });
                }
            }
        }

        [Serializable]
        public class Account : ISettingContent {
            
            
            public void Init() {
            }

            public JObject ToJObject() {
                JObject jAccount = new();

                return jAccount;
            }

            public void LoadFromJObject(JObject jAccount) {
                if (jAccount == null) return;

                try {
                } catch (Exception ex) {
                    LogBuilder.Log(LogType.Error, "Account.LoadFromJObject", $"Failed to load Account.",
                        new[] { new LogElement("Exception", ex.ToString()) });
                }
            }
        }

        [Serializable]
        public class Graphic : ISettingContent {
            public RuntimeValue<bool> enableVsync = new(true);
            public RuntimeValue<int> targetFrameRate = new(60);
            public RuntimeValue<float> brightness = new(0.5f);

            public void Init() {
                enableVsync.ValueChanged += (value) => {
                    QualitySettings.vSyncCount = value ? 1 : 0;
                };
                targetFrameRate.ValueChanged += (value) => {
                    Application.targetFrameRate = value;
                };

                enableVsync.InvokeValueChanged();
                targetFrameRate.InvokeValueChanged();
            }

            public JObject ToJObject() {
                JObject jGraphic = new();

                jGraphic["enableVsync"] = enableVsync.Value;
                jGraphic["targetFrameRate"] = targetFrameRate.Value;
                jGraphic["brightness"] = brightness.Value;

                return jGraphic;
            }

            public void LoadFromJObject(JObject jGraphic) {
                if (jGraphic == null) return;

                try {
                    enableVsync.Value = jGraphic.TryGetValue("enableVsync", enableVsync.Value);
                    targetFrameRate.Value = jGraphic.TryGetValue("targetFrameRate", targetFrameRate.Value);
                    brightness.Value = jGraphic.TryGetValue("brightness", brightness.Value);
                } catch (Exception ex) {
                    LogBuilder.Log(LogType.Error, "Graphic.LoadFromJObject", $"Failed to load Graphic.",
                        new[] { new LogElement("Exception", ex.ToString()) });
                }
            }
        }

        [Serializable]
        public class Audio : ISettingContent {
            public RuntimeValue<float> bgmVolume = new(1f);
            public RuntimeValue<float> sfxVolume = new(1f);
            public RuntimeValue<bool> enableVibration = new(true);

            public void Init() {
                bgmVolume.ValueChanged += (value) => {
                    AudioManager.Instance.BgmVolume = value;
                };
                sfxVolume.ValueChanged += (value) => {
                    AudioManager.Instance.SfxVolume = value;
                };
                enableVibration.ValueChanged += (value) => {
                    AudioManager.Instance.EnableVibration = value;
                };

                bgmVolume.InvokeValueChanged();
                sfxVolume.InvokeValueChanged();
                enableVibration.InvokeValueChanged();
            }

            public JObject ToJObject() {
                JObject jAudio = new();

                jAudio["bgmVolume"] = bgmVolume.Value;
                jAudio["sfxVolume"] = sfxVolume.Value;
                jAudio["enableVibration"] = enableVibration.Value;

                return jAudio;
            }

            public void LoadFromJObject(JObject jAudio) {
                if (jAudio == null) return;

                try {
                    bgmVolume.Value = jAudio.TryGetValue("bgmVolume", bgmVolume.Value);
                    sfxVolume.Value = jAudio.TryGetValue("sfxVolume", sfxVolume.Value);
                    enableVibration.Value = jAudio.TryGetValue("enableVibration", enableVibration.Value);
                } catch (Exception ex) {
                    LogBuilder.Log(LogType.Error, "Audio.LoadFromJObject", $"Failed to load Audio.",
                        new[] { new LogElement("Exception", ex.ToString()) });
                }
            }
        }

        public static GameSettings Instance { get; private set; }

        public readonly Language language = new();
        public readonly Account account = new();
        public readonly Graphic graphic = new();
        public readonly Audio audio = new();

        private ISettingContent[] settingContents;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            settingContents = new ISettingContent[] {
                language,
                account,
                graphic,
                audio
            };

            foreach (ISettingContent content in settingContents) {
                content.Init();
            }

            AddSaveEvents();
        }

        public JObject ToJObject() {
            JObject jGameSetting = new();

            jGameSetting["language"] = language.ToJObject();
            jGameSetting["account"] = account.ToJObject();
            jGameSetting["graphic"] = graphic.ToJObject();
            jGameSetting["audio"] = audio.ToJObject();

            return jGameSetting;
        }

        public void LoadFromJObject(JObject jGameSetting) {
            if (jGameSetting == null) return;

            try {
                language.LoadFromJObject(jGameSetting.TryGetValue<JObject>("language", null));
                account.LoadFromJObject(jGameSetting.TryGetValue<JObject>("account", null));
                graphic.LoadFromJObject(jGameSetting.TryGetValue<JObject>("graphic", null));
                audio.LoadFromJObject(jGameSetting.TryGetValue<JObject>("audio", null));
            } catch (Exception ex) {
                LogBuilder.Log(LogType.Error, "GameSetting.LoadFromJObject", $"Failed to load GameSetting.",
                    new[] { new LogElement("Exception", ex.ToString()) });
            }
        }

        private void AddSaveEvents() {
            foreach (ISettingContent content in settingContents) {
                content.GetType().GetFields().Where(x => x.FieldType.GetInterfaces().Contains(typeof(IRuntimeValue)))
                    .ToArray().ForEach(field => {
                        IRuntimeValue runtimeValue = field.GetValue(content) as IRuntimeValue;
                        runtimeValue.ValueChangedSimple += () => {
                            SaveData.MarkAsDirty();
                        };
                    });
            }
        }
    }
}