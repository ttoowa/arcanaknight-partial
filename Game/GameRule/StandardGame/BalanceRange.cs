using System;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    public class BalanceRange {
        [Header("Field")]
        public float min = 0f;

        public float max = 1f;
        public float power = 1f;

        public BalanceRange(float min, float max, float power = 1f) {
            this.min = min;
            this.max = max;
            this.power = power;
        }

        public float Sample(float x) {
            return Mathf.Pow(x, power) * (max - min) + min;
        }

        public int SampleFloorInt(float x) {
            return Mathf.FloorToInt(Sample(x));
        }

        public int SampleRoundInt(float x) {
            return Mathf.RoundToInt(Sample(x));
        }
    }
}