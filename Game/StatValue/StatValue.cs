using System;

namespace ArcaneSurvivorsClient {
    [Serializable]
    public struct StatValue {
        public float ActualValue {
            get {
                switch (type) {
                    default:
                        return value;
                    case StatValueType.Percent:
                        return value * 0.01f;
                }
            }
        }

        public float value;
        public StatValueType type;
        public StatValueBenefitDirection benefitDirection;

        public StatValue(float value, StatValueType type, StatValueBenefitDirection benefitDirection) {
            this.value = value;
            this.type = type;
            this.benefitDirection = benefitDirection;
        }

        public override string ToString() {
            switch (type) {
                default:
                    return value.ToString("0.0");
                case StatValueType.Percent:
                    return $"{value.ToString("0.0")}%";
            }
        }

        public float Apply(float baseValue, bool isRelativePercent = true) {
            switch (type) {
                default:
                    return baseValue + value;
                case StatValueType.Percent:
                    return baseValue * ((isRelativePercent ? 1f : 0f) + value * 0.01f);
            }
        }
    }
}