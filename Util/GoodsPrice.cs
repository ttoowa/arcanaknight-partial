using System;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class GoodsPrice {
        public float Value { get; private set; }

        public float MaxValue {
            get => maxValue;
            set {
                maxValue = value;
                ApplyMaxValue();
            }
        }

        public float inflation;
        public bool reservedInflation;

        public readonly float OriginalValue;
        private float maxValue = float.MaxValue;

        public int ActualValue => Mathf.FloorToInt(Value);

        public string DisplayValue => ActualValue.ToString();

        public GoodsPrice(float value, float inflation) {
            OriginalValue = value;
            Value = value;
            this.inflation = inflation;
        }

        public void Reset() {
            Value = OriginalValue;
        }

        public void InflateIfReserved() {
            if (!reservedInflation) return;
            reservedInflation = false;

            Value = Value * (1f + inflation);
            ApplyMaxValue();
        }

        public void ReserveInflate() {
            reservedInflation = true;
        }

        private void ApplyMaxValue() {
            Value = Mathf.Min(Value, MaxValue);
        }
    }
}