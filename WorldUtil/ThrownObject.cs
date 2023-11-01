using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ArcaneSurvivorsClient {
    [RequireComponent(typeof(Rigidbody))]
    public class ThrownObject : MonoBehaviour, IPauseable {
        public enum RotateMode {
            None,
            ByForce,
            ByVelocityNormal
        }

        public RotateMode rotateMode;
        public bool useGround = true;

        public float forceY = 10f;
        public float forceX = 1.5f;
        public float gravityFactor = 1f;

        [Range(0f, 1f)]
        public float bounce = 0.5f;


        private float groundY;
        private Vector2 velocity;
        private float angle;
        private float rotateSpeed;

        private void Start() {
            groundY = transform.localPosition.z;
            velocity.x = Random.Range(-1f, 1f) * forceX;
            velocity.y = forceY + Random.Range(-1f, 1f) * forceY * 0.15f;

            if (rotateMode == RotateMode.ByForce)
                rotateSpeed = Random.Range(-1f, 1f) * 30f;
        }

        private void FixedUpdate() {
            Vector3 position = transform.localPosition;

            velocity.x *= 0.98f;
            velocity.y -= 9.8f * Time.fixedDeltaTime * gravityFactor;
            if (useGround && transform.localPosition.z <= groundY && velocity.y < 0f) {
                if (velocity.y < 1f)
                    velocity.y *= -bounce;
                else
                    velocity.y = 0f;

                position.z = groundY;
            }

            switch (rotateMode) {
                case RotateMode.ByForce:
                    rotateSpeed *= 0.98f;
                    angle += rotateSpeed;
                    transform.localEulerAngles = new Vector3(0f, angle);
                    break;
                case RotateMode.ByVelocityNormal:
                    angle = velocity.ToAngle();
                    transform.localEulerAngles = new Vector3(0f, angle);
                    break;
            }

            position.x += velocity.x * Time.fixedDeltaTime;
            position.z += velocity.y * Time.fixedDeltaTime;
            transform.localPosition = position;
        }
    }
}