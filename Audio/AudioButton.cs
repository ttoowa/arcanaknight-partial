using System;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient {
    [RequireComponent(typeof(Button))]
    public class AudioButton : MonoBehaviour {
        public string audioKey;

        private Button button;

        private void Awake() {
            button = GetComponent<Button>();
        }

        private void Start() {
            button.onClick.AddListener(() => {
                SfxPlayer.Play(audioKey);
            });
        }
    }
}