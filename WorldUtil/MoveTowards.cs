using System;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class MoveTowards : MonoBehaviour {
        public float speed;
        public Vector3 normal;

        private void FixedUpdate() {
            transform.localPosition += normal.normalized * speed * Time.deltaTime;
        }
    }
}