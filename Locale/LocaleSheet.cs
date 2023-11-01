using System.Collections;
using System.Collections.Generic;

namespace ArcaneSurvivorsClient.Locale {
    public class LocaleSheet : IEnumerable<KeyValuePair<string, string>> {
        private readonly Dictionary<string, string> wordDict = new();

        public readonly string Language;

        public LocaleSheet(string language) {
            Language = language;
        }

        public void Clear() {
            wordDict.Clear();
        }

        public void Add(string key, string content) {
            wordDict.Add(key, content);
        }

        public string GetString(string key, params string[] parameters) {
            if (wordDict.ContainsKey(key)) {
                string content = wordDict[key];

                if (parameters != null) {
                    for (int i = 0; i < parameters.Length; ++i) {
                        string parameter = parameters[i];
                        if (parameter == null)
                            parameter = "[NULL PARAM]";
                        content = content.Replace($"{{{i}}}", parameter);
                    }
                }

                return content;
            } else
                return null;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {
            return wordDict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}