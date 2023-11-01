using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class VariableText : MonoBehaviour {
        private TextMeshProUGUI tmp;

        [SerializeField]
        private string fieldName;

        [SerializeField]
        private string defaultValue;
        
        private void Awake() {
            tmp = GetComponent<TextMeshProUGUI>();
        }

        private void Start() {
            tmp.text = VariableTextStorage.GetValue(fieldName, defaultValue);
            VariableTextStorage.AddListener(fieldName, OnValueChanged);
        }

        private void OnDestroy() {
            VariableTextStorage.RemoveListener(fieldName, OnValueChanged);
        }

        private void OnValueChanged(string value) {
            tmp.text = value;
        }
    }
}