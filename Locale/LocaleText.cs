using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient.Locale {
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocaleText : MonoBehaviour {
        public delegate void TextProcessingDelegate(ref string text);

        public string Key {
            get => key;
            set {
                key = value;
                UpdateText();
            }
        }

        public string[] Parameters {
            get => parameters;
            set {
                parameters = value;
                UpdateText();
            }
        }

        public string SpecificLanguage {
            get => specificLanguage;
            set {
                specificLanguage = value;
                UpdateText();
            }
        }

        public string ActualLanguage { get; private set; }

        public Color Color {
            get {
                if (textUI == null)
                    textUI = GetComponent<TextMeshProUGUI>();
                return textUI.color;
            }
            set {
                if (textUI == null)
                    textUI = GetComponent<TextMeshProUGUI>();
                textUI.color = value;
            }
        }

        [Header("{n} (Ex. {0}) 형식으로 작성 시 parameter로 대체되어 적용됩니다.")]
        [SerializeField]
        private string key;

        [FormerlySerializedAs("parameter")]
        [SerializeField]
        private string[] parameters;

        [SerializeField]
        private string specificLanguage;

        private TextMeshProUGUI textUI;

        public event TextProcessingDelegate TextProcessing;

        private void Awake() {
            textUI = GetComponent<TextMeshProUGUI>();
        }

        private void Start() {
            UpdateText();

            LocaleManager.Instance.LanguageChanged += OnLanguageChanged;
        }

        private void OnDestroy() {
            LocaleManager.Instance.LanguageChanged -= OnLanguageChanged;
        }

        private void OnLanguageChanged(LocaleSheet localesheet) {
            UpdateText();
        }

        private void UpdateText() {
            if (textUI == null)
                textUI = GetComponent<TextMeshProUGUI>();
            if (string.IsNullOrEmpty(key)) {
                textUI.text = "[INVALID KEY]";
                return;
            }

            string content = null;

            string[] actualParameters = GetActualParameters();
            if (string.IsNullOrWhiteSpace(specificLanguage)) {
                ActualLanguage = LocaleManager.Instance.CurrentLanguage;
                content = LocaleManager.GetString(key, actualParameters);
            } else {
                ActualLanguage = specificLanguage;
                content = LocaleManager.Instance.GetOrCreateLocaleSheet(specificLanguage)
                    ?.GetString(key, actualParameters);
            }

            if (content == null) {
                textUI.text = $"[NOTFOUND '{ActualLanguage}:{key}']";
                return;
            }

            TextProcessing?.Invoke(ref content);

            textUI.text = content;
        }

        private string[] GetActualParameters() {
            string[] actualParams = new string[parameters.Length];
            for (int i = 0; i < parameters.Length; ++i) {
                string param = parameters[i];
                if (param.StartsWith("{") && param.EndsWith("}")) {
                    string key = param.Substring(1, param.Length - 2);
                    param = LocaleManager.GetString(key);
                }

                actualParams[i] = param;
            }

            return actualParams;
        }
    }
}