using System;
using System.Collections;
using ArcaneSurvivorsClient.Analytics;
using Firebase.Analytics;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class MainMenuUI : MonoBehaviour {
        public static MainMenuUI Instance { get; private set; }

        [SerializeField]
        private GameObject panel;

        [SerializeField]
        private MainMenuTabGroup[] tabGroups;

        [SerializeField]
        private Animator contentAminator;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            SetVisible(false);

            foreach (MainMenuTabGroup tabGroup in tabGroups) {
                MainMenuTabGroup tabGroupInstance = tabGroup;
                tabGroup.tabButton.Clicked += () => {
                    SetActiveTab(tabGroupInstance);
                };
            }

            SetActiveTab(tabGroups[2]);
        }

        public void SetVisible(bool visible) {
            panel.SetActive(visible);

            if (visible) {
                BgmPlayer.Play("main");

                GameAnalytics.LogEvent("SceneEntered", new Parameter("name", "MainMenu"));

                SetActiveTab(GetTabGroup("play"));
            }
        }

        public void SetActiveTab(MainMenuTabGroup tabGroup) {
            foreach (MainMenuTabGroup group in tabGroups) {
                group.SetSelected(group == tabGroup);
            }

            switch (tabGroup.id) {
                case "play":
                    GameModeSelectorUI.Instance.SetFlow(GameModeSelectFlow.Mode);
                    break;
            }

            ReplayContentEntryAnimation();
        }

        public MainMenuTabGroup GetTabGroup(string id) {
            foreach (MainMenuTabGroup tabGroup in tabGroups) {
                if (tabGroup.id == id)
                    return tabGroup;
            }

            return null;
        }

        public void ReplayContentEntryAnimation() {
            contentAminator.SetTrigger("Replay");
        }
    }
}