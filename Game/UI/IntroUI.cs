using System;
using ArcaneSurvivorsClient.Analytics;
using Firebase.Analytics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ArcaneSurvivorsClient.Game {
    public class IntroUI : MonoBehaviour {
        public static IntroUI Instance { get; private set; }

        [SerializeField]
        private GameObject panel;

        [SerializeField]
        private EventTrigger eventTrigger;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            EventTrigger.Entry entry = new();
            entry.eventID = EventTriggerType.PointerClick;

            entry.callback.AddListener(eventData => {
                ScreenTransition.Show(ScreenTransitionType.AlphaFade, ScreenTransitionType.AlphaFade, () => {
                    SetVisible(false);
                    MainMenuUI.Instance.SetVisible(true);
                });
            });
            eventTrigger.triggers.Add(entry);
        }

        public void SetVisible(bool visible) {
            panel.SetActive(visible);

            if (visible) {
                BgmPlayer.Play("main");

                GameAnalytics.LogEvent("SceneEntered", new Parameter("name", "Intro"));
            }
        }
    }
}