using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [CustomEditor(typeof(UpgradableStat))]
    public class UpgradableStatEditor : Editor {
        private UpgradableStat value;

        private void OnEnable() {
            value = (UpgradableStat)target;
        }

        public override void OnInspectorGUI() {
            EditorGUILayout.BeginVertical();
            base.OnInspectorGUI();

            // Button
            if (GUILayout.Button("Simulate")) {
                int sum = 0;
                StringBuilder builder = new();
                builder.AppendLine($"Price increase simulation for '{value.name}'");
                for (int level = 0; level < value.maxLevel; ++level) {
                    int price = value.GetPrice(level);
                    sum += price;
                    builder.AppendLine($"  Level {level} : {price} G");
                }

                builder.AppendLine($"Total : {sum} G");

                Debug.Log(builder.ToString());
            }

            EditorGUILayout.EndVertical();
        }
    }
}