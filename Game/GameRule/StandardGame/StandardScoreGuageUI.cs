using System;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class StandardScoreGuageUI : MonoBehaviour {
        public struct ScoreGuageInfo {
            public float min;
            public float target;
            public float max;
            public float score;
        }

        public ScoreGuageInfo ScoreInfo =>
            new() {
                min = min,
                target = target,
                max = max,
                score = score
            };

        public RectTransform leftFore;
        public RectTransform rightFore;

        private float min;
        private float target = 1f;
        private float max = 1.5f;

        private float score;

        public void SetScore(float score) {
            this.score = score;
            float leftAmount = Mathf.Clamp01((score - min) / (target - min));
            float rightAmount = Mathf.Clamp01((score - target) / (max - target));
            leftFore.SetGuageAmount(leftAmount);
            rightFore.SetGuageAmount(rightAmount);
        }

        public void SetEnvironment(float min, float target, float max) {
            this.min = min;
            this.target = target;
            this.max = max;

            Debug.Log($"ScoreEnv : {min}, {target}, {max}");
        }

        public void SetForeColor(Color color) {
            leftFore.GetComponent<Image>().color = color;
            rightFore.GetComponent<Image>().color = color;
        }
    }
}