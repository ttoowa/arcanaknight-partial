using System;
using ArcaneSurvivorsClient.Locale;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient {
    [ExecuteInEditMode]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class AccentText : MonoBehaviour {
        public string srcText = @"<accent>";
        public Color32 color = Color.red;

        private const string dstText = @"<color=#{colorCode}>";

        private TextMeshProUGUI textUI;


        private void Start() {
            textUI = GetComponent<TextMeshProUGUI>();

            LocaleText localeText = GetComponent<LocaleText>();
            if (localeText != null)
                localeText.TextProcessing += ProcessText;

            Update();
        }

        private void Update() {
#if UNITY_EDITOR
            if (!Application.isPlaying) {
                if (textUI == null)
                    textUI = GetComponent<TextMeshProUGUI>();
            }
#endif

            string content = textUI.text;
            ProcessText(ref content);
            textUI.text = content;
        }

        private void ProcessText(ref string text) {
            if (!textUI.text.Contains(srcText)) return;

            string endSrcText = srcText.Replace(@"<", @"</");
            string endDstText = dstText.Replace(@"<", @"</");

            int attrIndex = endDstText.IndexOf('=');
            if (attrIndex >= 0)
                endDstText = endDstText.Substring(0, attrIndex) + ">";

            string actualDstText = dstText.Replace("{colorCode}", color.ToHex());
            text = textUI.text.Replace(srcText, actualDstText).Replace(endSrcText, endDstText);
        }
    }
}