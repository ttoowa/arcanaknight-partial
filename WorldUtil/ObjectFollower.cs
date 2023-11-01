using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public enum FollowMode {
        Immediate,
        Smooth,
        Linear
    }


    public class ObjectFollower : MonoBehaviour {
        private interface IFollower {
            Vector3 FollowTick(Vector3 src, Vector3 target, float delta);
        }

        private class ImmediateFollower : IFollower {
            public Vector3 FollowTick(Vector3 src, Vector3 target, float delta) {
                return target;
            }
        }

        private class SmoothFollower : IFollower {
            public Vector3 FollowTick(Vector3 src, Vector3 target, float delta) {
                return src + (target - src) * Mathf.Clamp01(delta);
            }
        }

        private class LinearFollower : IFollower {
            public Vector3 FollowTick(Vector3 src, Vector3 target, float delta) {
                return Vector3.MoveTowards(src, target, delta);
            }
        }

        public Transform target;
        public Vector3 offset;
        [Range(0, 1)] public float delta = 0.15f;
        public FollowMode mode = FollowMode.Smooth;

        private IFollower follower;

        public event Func<Transform> FindTarget;

        private void Start() {
            SetMode(mode);
        }

        private void FixedUpdate() {
            if (target == null && FindTarget != null)
                target = FindTarget();

            if (target == null)
                return;

            transform.position = follower.FollowTick(transform.position, target.position + offset, delta);
        }

        public void SetMode(FollowMode mode) {
            switch (mode) {
                default:
                case FollowMode.Immediate:
                    follower = new ImmediateFollower();
                    break;
                case FollowMode.Smooth:
                    follower = new SmoothFollower();
                    break;
                case FollowMode.Linear:
                    follower = new LinearFollower();
                    break;
            }
        }
    }
}