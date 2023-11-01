using System.Collections.Generic;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public static class PhysicsUtility {
        private const float SimulationFps = 60f;
        private const float SimulationFrameTick = 1f / SimulationFps;
        private const float Tolerance = 0.00005f;

        private static Dictionary<Vector2, float> moveForceDict = new();

        public static float CalcMoveForce(float targetSpeed, float friction) {
            Vector2 dictKey = new(targetSpeed, friction);
            if (moveForceDict.ContainsKey(dictKey))
                return moveForceDict[dictKey];

            float moveForce = SearchUtility.BinarySearch((float moveForce) => {
                return CalcMaxSpeed(moveForce, friction);
            }, targetSpeed / SimulationFps, 0f, 1000f, Tolerance, 1000);
            moveForceDict[dictKey] = moveForce;

            return moveForce;
        }

        public static float CalcMaxSpeed(float moveForce, float friction) {
            float speed = 0f;
            for (int i = 0; i < 1000; ++i) {
                float newSpeed = speed + moveForce * SimulationFrameTick;
                newSpeed *= 1f - Mathf.Clamp01(friction);

                if (Mathf.Abs(newSpeed - speed) <= Tolerance) {
                    speed = newSpeed;

                    break;
                }

                speed = newSpeed;
            }

            return speed;
        }
    }
}