using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class FitUIManager : MonoBehaviour {
        public static FitUIManager Instance { get; private set; }

        private List<FitToText> textList = new();
        private List<FitToContent> contentList = new();

        private void Awake() {
            Instance = this;
        }

        private void LateUpdate() {
            foreach (FitToText text in textList) {
                if (text.gameObject.activeInHierarchy)
                    text.Fit();
            }

            // Sort contents by hierarchy depth
            Dictionary<FitToContent, int> depthDict = new();
            contentList.Sort((left, right) => {
                int leftDepth;
                int rightDepth;
                if (depthDict.ContainsKey(left))
                    leftDepth = depthDict[left];
                else {
                    leftDepth = left.transform.Depth();
                    depthDict[left] = leftDepth;
                }

                if (depthDict.ContainsKey(right))
                    rightDepth = depthDict[right];
                else {
                    rightDepth = right.transform.Depth();
                    depthDict[right] = rightDepth;
                }

                return leftDepth.CompareTo(rightDepth);
            });

            for (int i = contentList.Count - 1; i >= 0; --i) {
                FitToContent content = contentList[i];
                if (content.transform.gameObject.activeInHierarchy)
                    content.Fit();
            }
        }

        public void AddText(FitToText text) {
            textList.Add(text);
        }

        public void AddContent(FitToContent content) {
            contentList.Add(content);
        }

        public void RemoveText(FitToText text) {
            textList.Remove(text);
        }

        public void RemoveContent(FitToContent content) {
            contentList.Remove(content);
        }
    }
}