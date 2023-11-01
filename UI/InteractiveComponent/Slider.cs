using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient {
    public class Slider : MonoBehaviour {
        public delegate void ValueChangedDelegate(float value);

        public float Value { get; private set; }

        public Image foreImage;
        public Image backImage;
        public EventTrigger eventTrigger;

        public event ValueChangedDelegate ValueChanged;

        private void Awake() {
            EventTrigger.Entry dragEntry = new();
            dragEntry.eventID = EventTriggerType.Drag;
            dragEntry.callback.AddListener((data) => {
                OnDrag((PointerEventData)data);
            });

            EventTrigger.Entry pointerDownEntry = new();
            pointerDownEntry.eventID = EventTriggerType.PointerDown;
            pointerDownEntry.callback.AddListener((data) => {
                OnDrag((PointerEventData)data);
            });

            eventTrigger.triggers.Add(dragEntry);
            eventTrigger.triggers.Add(pointerDownEntry);
        }

        public void OnValueChanged(float value) {
            ValueChanged?.Invoke(value);
        }

        private void OnDrag(PointerEventData data) {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(backImage.rectTransform, data.position,
                data.pressEventCamera, out localPoint);

            float value = Mathf.Clamp01(localPoint.x / backImage.rectTransform.sizeDelta.x);

            SetValue(value);
        }

        public void SetValue(float value, bool withEvent = true) {
            Value = value;
            foreImage.fillAmount = value;

            if (withEvent)
                OnValueChanged(value);
        }
    }
}