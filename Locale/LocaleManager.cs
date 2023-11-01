using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ArcaneSurvivorsClient.Locale {
    public class LocaleManager : MonoBehaviour {
        public delegate void LanguageChangedDelegate(LocaleSheet localeSheet);

        public static LocaleManager Instance { get; private set; }

        public string[] Languages => sheetDict.Keys.ToArray();

        private readonly Dictionary<string, LocaleSheet> sheetDict = new();

        public string CurrentLanguage { get; private set; }
        public LocaleSheet CurrentLocaleSheet { get; private set; }

        [SerializeField]
        private TextAsset localeSheetAsset;

        public event LanguageChangedDelegate LanguageChanged;

        private void Awake() {
            Instance = this;

            LoadLocaleSheet();
            SetLanguage("English");
        }

        private void LoadLocaleSheet() {
            JObject jLocaleFile = JObject.Parse(localeSheetAsset.text);
            foreach (KeyValuePair<string, JToken> jLocaleSheetPair in jLocaleFile) {
                string language = jLocaleSheetPair.Key;
                JObject jLocaleSheet = jLocaleSheetPair.Value as JObject;

                LocaleSheet localeSheet = GetOrCreateLocaleSheet(language);

                foreach (KeyValuePair<string, JToken> jWordPair in jLocaleSheet) {
                    string key = jWordPair.Key;
                    string content = jWordPair.Value.ToString();

                    localeSheet.Add(key, content);
                }
            }
        }

        public LocaleSheet GetOrCreateLocaleSheet(string language) {
            language = NormalizeLanguageName(language);
            if (sheetDict.ContainsKey(language))
                return sheetDict[language];

            LocaleSheet localeSheet = new(language);
            sheetDict.Add(language, localeSheet);
            return localeSheet;
        }

        public void SetLanguage(string language) {
            language = NormalizeLanguageName(language);
            CurrentLanguage = language;
            CurrentLocaleSheet = GetOrCreateLocaleSheet(language);

            LanguageChanged?.Invoke(CurrentLocaleSheet);
        }

        public static string GetString(string localeKey, params string[] parameters) {
            string content = Instance.CurrentLocaleSheet.GetString(localeKey, parameters);
            if (content == null) {
                Debug.LogWarning(LogBuilder.Build(LogType.Warning, "LocaleManager",
                    $"Locale key not found: {localeKey}", new LogElement("Language", Instance.CurrentLanguage)));
                return localeKey;
            }

            return content;
        }

        public static string NormalizeLanguageName(string language) {
            return language.ToLower();
        }
    }
}