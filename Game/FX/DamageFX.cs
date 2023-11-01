using System;
using TMPro;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class DamageFX : MonoBehaviour {
        private const float duration = 1f;

        public Transform graphic;
        public SpriteFontRenderer text;

        public AnimationCurve positionXCurve;
        public AnimationCurve positionZCurve;
        public AnimationCurve scaleZCurve;
        public AnimationCurve alphaCurve;

        private float elapsedTime;
        private Vector3 position;
        private Vector3 scale;

        public void SetDamage(int damage) {
            text.Text = damage.ToString();
        }

        public void SetVisible(bool visible) {
            gameObject.SetActive(visible);
        }

        public void RestartAnimation() {
            elapsedTime = 0f;
            position = Vector3.zero;
            scale = Vector3.one;
        }

        private void Start() {
            RestartAnimation();
        }

        private void Update() {
            elapsedTime += Time.deltaTime;

            float normalizedTime = Mathf.Clamp01(elapsedTime / duration);

            position.x = positionXCurve.Evaluate(normalizedTime);
            position.z = positionZCurve.Evaluate(normalizedTime);
            scale.z = scaleZCurve.Evaluate(normalizedTime);
            text.Color = new Color(text.Color.r, text.Color.g, text.Color.b, alphaCurve.Evaluate(normalizedTime));

            graphic.localPosition = position;
            graphic.localScale = scale;
        }
    }
}