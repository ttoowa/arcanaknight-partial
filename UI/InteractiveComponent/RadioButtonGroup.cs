using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class RadioButtonGroup : MonoBehaviour {
        public delegate void SelectedStateChangedDelegate(RadioButton radioButton, bool isSelected);

        public RadioButton SelectedButton { get; private set; }

        public event SelectedStateChangedDelegate SelectedStateChanged;

        public void OnSelectedStateChanged(RadioButton radioButton, bool isSelected) {
            if (isSelected) {
                RadioButton[] buttons = GetComponentsInChildren<RadioButton>();

                foreach (RadioButton button in buttons) {
                    button.SetSelected(button == radioButton, false);
                }
            }

            SelectedStateChanged?.Invoke(radioButton, isSelected);
        }

        public void Select(string value, bool withEvent = true) {
            RadioButton[] buttons = GetComponentsInChildren<RadioButton>();

            foreach (RadioButton button in buttons) {
                bool match = button.value.ToLower() == value.ToLower();
                button.SetSelected(match, withEvent);
            }
        }
    }
}