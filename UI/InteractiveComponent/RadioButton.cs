using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient {
    [RequireComponent(typeof(Button))]
    public class RadioButton : MonoBehaviour {
        public delegate void SelectedStateChangedDelegate(bool isSelected);

        public bool BoolValue => value.ToLower() == "true";
        public int IntValue => int.Parse(value);
        public float FloatValue => float.Parse(value, CultureInfo.InvariantCulture);
        public string StringValue => value;

        public Image image;
        public Sprite[] sprites;
        public string value;

        private Button button;

        public event SelectedStateChangedDelegate SelectedStateChanged;

        private void Awake() {
            button = GetComponent<Button>();

            button.onClick.AddListener(() => {
                SetSelected(true);
            });
        }

        public void SetSelected(bool isSelected, bool withEvent = true) {
            image.sprite = sprites[isSelected ? 0 : 1];

            if (withEvent) {
                RadioButtonGroup group = GetComponentInParent<RadioButtonGroup>();
                group.OnSelectedStateChanged(this, isSelected);

                SelectedStateChanged?.Invoke(isSelected);
            }
        }
    }
}