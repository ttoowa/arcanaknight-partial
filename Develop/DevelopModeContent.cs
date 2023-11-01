using System;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class DevelopModeContent : MonoBehaviour {
        private void Awake() {
            if (!DevelopSetting.IsDevelopMode)
                Destroy(gameObject);
        }
    }
}