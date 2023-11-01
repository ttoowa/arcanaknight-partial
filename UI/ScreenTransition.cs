using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient {
    public class ScreenTransition : MonoBehaviour, ISingletone {
        public static ScreenTransition Instance { get; private set; }

        public Image transitionPanel;

        [Header("Transition Material")]
        public Material alphaFade;

        public Material zoomCircle;
        public Material slideFloor;
        public Material slideGradient;
        public Material slideDiamond;

        private EasingPreset easingPreset;

        private const float DurationHalf = 0.4f;

        private void Awake() {
            Instance = this;
        }

        public static void Show(ScreenTransitionType fadeType, Action action) {
            Show(fadeType, fadeType, action);
        }

        public static void Show(ScreenTransitionType fadeInType, ScreenTransitionType fadeOutType, Action action,
            float fadeOutPreDelay = 0f, float fadeOutPostDelay = 0.1f) {
            Instance.Show_(fadeInType, fadeOutType, action, fadeOutPreDelay, fadeOutPostDelay);
        }

        public static void Show(ScreenTransitionType fadeInType, ScreenTransitionType fadeOutType,
            IEnumerator coroutineLogic, float fadeOutPreDelay = 0f, float fadeOutPostDelay = 0.1f) {
            Instance.Show_(fadeInType, fadeOutType, coroutineLogic, fadeOutPreDelay, fadeOutPostDelay);
        }

        private void Show_(ScreenTransitionType fadeInType, ScreenTransitionType fadeOutType, Action action,
            float fadeOutPreDelay, float fadeOutPostDelay) {
            IEnumerator InvokeAction() {
                action?.Invoke();
                yield break;
            }

            CoroutineDispatcher.Dispatch(FadeTransition(fadeInType, fadeOutType, InvokeAction(), fadeOutPreDelay,
                fadeOutPostDelay));
        }

        private void Show_(ScreenTransitionType fadeInType, ScreenTransitionType fadeOutType,
            IEnumerator coroutineLogic, float fadeOutPreDelay, float fadeOutPostDelay) {
            CoroutineDispatcher.Dispatch(FadeTransition(fadeInType, fadeOutType, coroutineLogic, fadeOutPreDelay,
                fadeOutPostDelay));
        }

        private IEnumerator FadeTransition(ScreenTransitionType fadeInType, ScreenTransitionType fadeOutType,
            IEnumerator coroutineLogic, float fadeOutPreDelay, float fadeOutPostDelay) {
            ApplyMaterial(fadeInType);
            transitionPanel.gameObject.SetActive(true);
            transitionPanel.material.SetFloat("_ProgressIn", 0f);
            transitionPanel.material.SetFloat("_ProgressOut", 0f);

            if (fadeInType != ScreenTransitionType.Flash) {
                for (float t = 0f; t < 1f; t += Time.deltaTime / DurationHalf) {
                    float progress = Easing.EasePreset(t, easingPreset);
                    transitionPanel.material.SetFloat("_ProgressIn", progress);
                    yield return null;
                }
            }

            transitionPanel.material.SetFloat("_ProgressIn", 1f);

            //float waitDuration = fadeInType == fadeOutType ? 0.1f : 0.3f;
            Debug.Log($"PreDelay : {fadeOutPreDelay}");
            yield return new WaitForSeconds(fadeOutPreDelay);
            yield return CoroutineDispatcher.Dispatch(coroutineLogic);
            Debug.Log($"PostDelay : {fadeOutPostDelay}");
            yield return new WaitForSeconds(fadeOutPostDelay);

            ApplyMaterial(fadeOutType);
            if (fadeInType != ScreenTransitionType.Flash) {
                transitionPanel.material.SetFloat("_ProgressIn", 1f);
                transitionPanel.material.SetFloat("_ProgressOut", 0f);
                for (float t = 0f; t < 1f; t += Time.deltaTime / DurationHalf) {
                    float progress = Easing.EasePreset(t, easingPreset);
                    transitionPanel.material.SetFloat("_ProgressOut", progress);
                    yield return null;
                }
            }

            transitionPanel.gameObject.SetActive(false);
        }

        private void ApplyMaterial(ScreenTransitionType type) {
            Material material;
            switch (type) {
                default:
                case ScreenTransitionType.AlphaFade:
                    material = alphaFade;
                    easingPreset = EasingPreset.EaseInQuad;
                    break;
                case ScreenTransitionType.ZoomCircle:
                    material = zoomCircle;
                    easingPreset = EasingPreset.EaseInQuad;
                    break;
                case ScreenTransitionType.SlideFloor:
                    material = slideFloor;
                    easingPreset = EasingPreset.Linear;
                    break;
                case ScreenTransitionType.SlideGradient:
                    material = slideGradient;
                    easingPreset = EasingPreset.EaseInQuad;
                    break;
                case ScreenTransitionType.SlideDiamond:
                    material = slideDiamond;
                    easingPreset = EasingPreset.Linear;
                    break;
            }

            transitionPanel.material = Instantiate(material);
        }
    }
}