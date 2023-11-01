using System;
using System.Collections;
using ArcaneSurvivorsClient.Locale;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient {
    public class PlatformLoginUI : MonoBehaviour {
        public static PlatformLoginUI Instance { get; private set; }

        public GameObject panel;
        public Button loginButton;

        private void Awake() {
            Instance = this;

            SetVisible(false, false);
        }

        private void Start() {
            loginButton.onClick.AddListener(() => {
                StartCoroutine(AuthRoutine());
            });
        }

        public void SetVisible(bool visible, bool withFade = true) {
            panel.SetActive(visible);

            if (visible)
                StartCoroutine(AuthRoutine());

            if (withFade) {
                if (visible)
                    FadeStack.Push();
                else
                    FadeStack.Pop();
            }
        }

        private IEnumerator AuthRoutine() {
            if (MobilePlatformManager.IsLoggedIn) {
                SetVisible(false);
                yield break;
            }

            LoadingUI.Push();

#if UNITY_EDITOR
            MobilePlatformManager.IsLoggedIn = true;
#else
            yield return MobilePlatformManager.AuthenticateAccount().AsCoroutine();
#endif

            LoadingUI.Pop();

            if (!MobilePlatformManager.IsLoggedIn) {
                ToastMessage.Show("alert.failedToLogin".ToLocale());
                yield break;
            }

            SetVisible(false);
        }
    }
}