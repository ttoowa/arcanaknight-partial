using System;
using System.Collections;
using ArcaneSurvivorsClient.Game;
using ArcaneSurvivorsClient;
using ArcaneSurvivorsClient.Analytics;
using ArcaneSurvivorsClient.Service;
using UnityEngine;
using Firebase;
using Firebase.Analytics;

namespace ArcaneSurvivorsClient {
    public class EntryPoint : MonoBehaviour, ISingletone {
        public static EntryPoint Instance { get; private set; }

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            StartCoroutine(EntryRoutine());
        }

        private IEnumerator EntryRoutine() {
            for (int i = 0; i < 10; ++i) {
                yield return null;
            }

            IntroUI.Instance.SetVisible(true);
            PlatformLoginUI.Instance.SetVisible(true);

            yield return GameAnalytics.Init().AsCoroutine();
            yield return CoroutineUtility.WaitBoolRoutine(() => {
                return MobilePlatformManager.IsLoggedIn;
            });

            GameAnalytics.LogEvent("ApplicationStarted");

            LoadingUI.Push();

            yield return SaveData.Load().AsCoroutine();

            LoadingUI.Pop();

            if (!TermsAndConditions.Instance.isCollectedAnswers.Value)
                TermsAndConditionsUI.Instance.SetVisible(true);
        }
    }
}