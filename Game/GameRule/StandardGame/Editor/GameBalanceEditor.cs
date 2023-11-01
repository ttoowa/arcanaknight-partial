using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [CustomEditor(typeof(GameBalance))]
    public class GameBalanceEditor : Editor {
        private const int SampleCount = 20;
        private static Vector2 GraphSize = new(300, 100);

        private GameBalance value;
        private readonly List<Vector3> pointList = new();

        private void OnEnable() {
            value = (GameBalance)target;
        }

        public override void OnInspectorGUI() {
            EditorGUILayout.BeginVertical();
            base.OnInspectorGUI();

            Vector2 graphSize = new(EditorGUIUtility.currentViewWidth - 50, GraphSize.y);

            EditorGUILayout.Space(8);
            foreach (FieldInfo fieldInfo in typeof(GameBalance).GetFields()) {
                if (fieldInfo.FieldType == typeof(BalanceRange)) {
                    EditorGUILayout.LabelField(fieldInfo.Name);
                    DrawGraph(fieldInfo.GetValue(value) as BalanceRange, graphSize);
                }
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawGraph(BalanceRange range, Vector2 graphSize) {
            Rect rect = GUILayoutUtility.GetRect(graphSize.x, graphSize.x, graphSize.y, graphSize.y);
            GUI.BeginClip(rect);

            pointList.Clear();
            for (int sampleI = 0; sampleI < SampleCount; ++sampleI) {
                float t = (float)sampleI / (SampleCount - 1);

                float score = Mathf.Pow(t, range.power);
                pointList.Add(new Vector3(t * graphSize.x, graphSize.y - score * graphSize.y, 0));
            }

            Handles.color = Color.white;
            Handles.DrawAAPolyLine(Texture2D.whiteTexture, 1, pointList.ToArray());

            Color guideColor = new(1f, 1f, 1f, 0.5f);
            Color accentColor = new(0f, 0.8f, 1f, 0.8f);
            Handles.color = guideColor;
            Handles.DrawAAPolyLine(Texture2D.whiteTexture, 1, Vector3.zero, new Vector3(graphSize.x, 0),
                new Vector3(graphSize.x, graphSize.y), new Vector3(0, graphSize.y), Vector3.zero);

            string numberFormat = range.max > 10 ? "0" : "0.0";
            for (int i = 0; i < value.previewSampleCount; ++i) {
                float t = (float)i / (value.previewSampleCount - 1);

                Vector3 pos = new(t * graphSize.x, graphSize.y);
                TextAnchor alignment = TextAnchor.LowerCenter;
                if (i == 0)
                    alignment = TextAnchor.LowerLeft;
                else if (i == value.previewSampleCount - 1)
                    alignment = TextAnchor.LowerRight;
                Handles.Label(pos + new Vector3(0, -20), $"#{i + 1}", new GUIStyle() {
                    alignment = alignment,
                    normal = new GUIStyleState() {
                        textColor = guideColor
                    }
                });
                Handles.Label(pos, range.Sample(t).ToString(numberFormat), new GUIStyle() {
                    alignment = alignment,
                    normal = new GUIStyleState() {
                        textColor = accentColor
                    }
                });
                Handles.DrawAAPolyLine(Texture2D.whiteTexture, 1, new Vector3(t * graphSize.x, graphSize.y),
                    new Vector3(t * graphSize.x, graphSize.y - Mathf.Pow(t, range.power) * graphSize.y));
            }

            GUI.EndClip();
            EditorGUILayout.Space(8);
        }
    }
}