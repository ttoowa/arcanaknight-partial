using System;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient {
    public delegate void CheckStateChanged(bool isChecked);

    [RequireComponent(typeof(Button))]
    public class Checkbox : MonoBehaviour {
        public delegate void CheckStateChangedDeleagate(bool isChecked);

        public Image image;
        public Sprite[] sprites;

        private Button button;

        public bool IsChecked { get; private set; }

        public event CheckStateChangedDeleagate CheckStateChanged;

        private void Awake() {
            button = GetComponent<Button>();

            button.onClick.AddListener(() => {
                SetChecked(!IsChecked);
            });
        }

        public void SetChecked(bool isChecked, bool withEvent = true) {
            IsChecked = isChecked;

            image.sprite = sprites[isChecked ? 0 : 1];

            if (withEvent)
                CheckStateChanged?.Invoke(isChecked);
        }
    }
}