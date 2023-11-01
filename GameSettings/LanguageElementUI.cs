using ArcaneSurvivorsClient.Locale;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient {
    public class LanguageElementUI : MonoBehaviour, ISelectableElement {
        public string Id { get; set; }

        public Image selectedBackPanel;
        public LocaleText languageText;
        public Color[] colors;

        public event ISelectableElement.SelectedStateChangedDelegate SelectedStateChanged;

        public void SetSelected(bool selected, bool withEvent = true) {
            languageText.Color = colors[selected ? 0 : 1];
            selectedBackPanel.enabled = selected;

            if (withEvent)
                SelectedStateChanged?.Invoke(selected);
        }
    }
}