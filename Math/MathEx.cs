using UnityEngine;

namespace ArcaneSurvivorsClient {
    public static class MathEx {
        public static float Decimal(float number) {
            return number - (int)number;
        }

        public static Vector3 ToNormal(this float angle) {
            return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0f, Mathf.Cos(angle * Mathf.Deg2Rad));
        }
    }
}