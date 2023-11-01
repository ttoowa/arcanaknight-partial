using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient {
    public class GameSettingsTabButtonUI : MonoBehaviour, ISelectableElement {
        public string Id { get; set; }

        public Image selectedBackPanel;
        public TextMeshProUGUI text;
        public Color[] textColors;

        public GameObject content;

        public event ISelectableElement.SelectedStateChangedDelegate SelectedStateChanged;

        public void SetSelected(bool selected, bool withEvent = true) {
            selectedBackPanel.enabled = selected;
            text.color = textColors[selected ? 0 : 1];
            content.SetActive(selected);

            if (withEvent)
                SelectedStateChanged?.Invoke(selected);
        }
    }
}