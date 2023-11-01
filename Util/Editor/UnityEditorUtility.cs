using UnityEditor;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public static class UnityEditorUtility {
        public static void ShowNotification(string message, bool withLog = false) {
            foreach (SceneView sceneView in SceneView.sceneViews) {
                sceneView?.ShowNotification(new GUIContent(message));
            }

            if (withLog)
                Debug.Log(message);
        }
    }
}