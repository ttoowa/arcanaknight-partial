using System;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient {
    [RequireComponent(typeof(Button))]
    public class InteractiveButton : MonoBehaviour {
        private Button button;

        private void Awake() {
            button = GetComponent<Button>();

            button.onClick.AddListener(() => {
                SfxPlayer.Play("button.default.click");
            });
        }
    }
}