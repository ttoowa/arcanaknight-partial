using System;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class Cameras : MonoBehaviour, ISingletone {
        public static Cameras Instance { get; private set; }

        public const float WorldSafeDistance = 24f;
        public Vector3 WorldCameraFocus => gimball_World.transform.position;

        public Camera camera_UI;
        public Camera camera_World;
        public Transform gimball_World;


        private void Awake() {
            Instance = this;
        }
    }
}