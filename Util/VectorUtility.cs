using UnityEngine;

namespace ArcaneSurvivorsClient {
    public static class VectorUtility {
        public enum Vector3ToVector2 {
            XYtoXY,
            XZtoXY
        }

        public enum Vector2ToVector3 {
            XYtoXY,
            XYtoXZ
        }

        public static Vector2 ToVector2(this Vector3 vector, Vector3ToVector2 mode = Vector3ToVector2.XZtoXY) {
            switch (mode) {
                default:
                case Vector3ToVector2.XYtoXY:
                    return new Vector2(vector.x, vector.y);
                case Vector3ToVector2.XZtoXY:
                    return new Vector2(vector.x, vector.z);
            }
        }

        public static Vector3 ToVector3(this Vector2 vector, Vector2ToVector3 mode = Vector2ToVector3.XYtoXZ) {
            switch (mode) {
                default:
                case Vector2ToVector3.XYtoXY:
                    return new Vector3(vector.x, vector.y, 0f);
                case Vector2ToVector3.XYtoXZ:
                    return new Vector3(vector.x, 0f, vector.y);
            }
        }

        public static Vector2 AddAngle(this Vector2 vector, float degreeAngle) {
            float vectorAngle = Mathf.Atan2(vector.x, vector.y);
            float vectorLength = vector.magnitude;

            vectorAngle -= degreeAngle * Mathf.Deg2Rad;

            return new Vector2(Mathf.Sin(vectorAngle), Mathf.Cos(vectorAngle)) * vectorLength;
        }

        public static float ToAngle(this Vector2 vector) {
            return Mathf.Atan2(vector.x, vector.y) * Mathf.Rad2Deg;
        }
    }
}