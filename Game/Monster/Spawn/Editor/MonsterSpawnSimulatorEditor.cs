using UnityEngine;
using UnityEditor;

namespace ArcaneSurvivorsClient.Game {
    [CustomEditor(typeof(MonsterSpawnSimulator))]
    [CanEditMultipleObjects]
    public class MonsterSpawnSimulatorEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            MonsterSpawnSimulator simulator = (MonsterSpawnSimulator)target;
            if (GUILayout.Button("Simulate"))
                simulator.Simulate();
        }
    }
}