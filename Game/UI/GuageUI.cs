using System;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class GuageUI : MonoBehaviour {
        private const float ShadowSpeed = 0.13f;

        [SerializeField]
        private Image fore;

        [SerializeField]
        private Image foreTail;

        public void SetValue(float value) {
            fore.fillAmount = value;

            OnValueChanged();
        }

        private void Update() {
            if (foreTail.fillAmount > fore.fillAmount)
                foreTail.fillAmount = Mathf.Max(foreTail.fillAmount - ShadowSpeed * Time.deltaTime, fore.fillAmount);
            else
                foreTail.fillAmount = fore.fillAmount;
        }

        private void OnValueChanged() {
            foreTail.fillAmount = Mathf.Max(foreTail.fillAmount, fore.fillAmount);
        }
    }
}