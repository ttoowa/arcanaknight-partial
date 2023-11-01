using System;
using System.Collections;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class SplashScreen : MonoBehaviour {
        public float logoFadeTime = 0.3f;
        public float logoTime = 0.3f;

        public GameObject panel;
        public CanvasGroup splashScreen;
        public CanvasGroup[] logos;

        private void Start() {
            if (Application.isEditor) {
                panel.SetActive(false);
                return;
            }

            CoroutineDispatcher.Dispatch(SplashScreenRoutine());
        }

        private IEnumerator SplashScreenRoutine() {
            panel.SetActive(true);
            splashScreen.alpha = 1f;
            foreach (CanvasGroup logo in logos) {
                logo.alpha = 0f;
            }

            yield return new WaitForSeconds(0.3f);
            for (int logoI = 0; logoI < logos.Length; ++logoI) {
                CanvasGroup logo = logos[logoI];

                for (float t = 0f; t < 1f; t += Time.deltaTime / logoFadeTime) {
                    logo.alpha = t;
                    yield return null;
                }

                logo.alpha = 1f;

                //yield return new WaitForSeconds(logoTime);

                for (float t = 0f; t < 1f; t += Time.deltaTime / logoFadeTime) {
                    logo.alpha = Mathf.Clamp01(1f - t);
                    yield return null;
                }

                logo.alpha = 0f;
            }

            for (float t = 0f; t < 1f; t += Time.deltaTime) {
                splashScreen.alpha = Mathf.Clamp01(1f - t);
                yield return null;
            }

            panel.SetActive(false);
        }
    }
}