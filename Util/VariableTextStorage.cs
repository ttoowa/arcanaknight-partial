using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class VariableTextStorage {
        private static Dictionary<string, RuntimeValue<string>> dict = new();

        public static void SetValue(string fieldName, string value) {
            RuntimeValue<string> entry = GetOrCreateEntry(fieldName);
            entry.Value = value;
        }

        public static string GetValue(string fieldName, string defaultValue = null) {
            RuntimeValue<string> entry = GetOrCreateEntry(fieldName, defaultValue);
            return entry.Value;
        }

        public static void AddListener(string fieldName, Action<string> onValueChanged) {
            RuntimeValue<string> entry = GetOrCreateEntry(fieldName);
            entry.ValueChanged += onValueChanged;
        }

        public static void RemoveListener(string fieldName, Action<string> onValueChanged) {
            RuntimeValue<string> entry = GetOrCreateEntry(fieldName);
            entry.ValueChanged -= onValueChanged;
        }

        private static RuntimeValue<string> GetOrCreateEntry(string fieldName, string defaultValue = null) {
            if (dict.TryGetValue(fieldName, out RuntimeValue<string> entry))
                return entry;
            else {
                RuntimeValue<string> newEntry = new(defaultValue);
                dict.Add(fieldName, newEntry);
                return newEntry;
            }
        }
    }
}