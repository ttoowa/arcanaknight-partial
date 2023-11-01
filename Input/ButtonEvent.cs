using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient {
    [RequireComponent(typeof(Button))]
    public class ButtonEvent : MonoBehaviour {
        public bool IsClicked => isClicked;

        [HideInInspector] public Button button;

        private bool isClicked;

        private void Awake() {
            button = GetComponent<Button>();

            button.onClick.AddListener(() => {
                isClicked = true;
                StartCoroutine(ClickedRoutine());
            });
        }

        private IEnumerator ClickedRoutine() {
            yield return null;
            isClicked = false;
        }
    }
}