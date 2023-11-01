using System;
using ArcaneSurvivorsClient.Locale;
using TMPro;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class StageChangeDirectingUI : MonoBehaviour {
        public static StageChangeDirectingUI Instance { get; private set; }

        public event Action AnimationComplete;
        public event Action AnimationCompleteOnce;

        private Action onFadeInComplete;

        [SerializeField]
        private GameObject panel;

        [SerializeField]
        private GeneralAnimator generalAnimator;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private LocaleText mapNameText;

        [SerializeField]
        private TextMeshProUGUI stageNameText;

        private void Awake() {
            Instance = this;

            panel.SetActive(false);
        }

        private void Start() {
            generalAnimator.AddEventListener(() => {
                onFadeInComplete?.Invoke();
                onFadeInComplete = null;
            }, 0);
            generalAnimator.AddEventListener(() => {
                AnimationComplete?.Invoke();
                AnimationCompleteOnce?.Invoke();
                AnimationCompleteOnce = null;

                panel.SetActive(false);
            }, 1);
        }

        public static void Show(string mapName, string stageName, Action onFadeInComplete = null) {
            Instance.Show_(mapName, stageName, onFadeInComplete);
        }

        private void Show_(string mapName, string stageName, Action onFadeInComplete) {
            mapNameText.Key = mapName;
            stageNameText.text = stageName;
            this.onFadeInComplete = onFadeInComplete;

            panel.SetActive(true);

            animator.SetTrigger("Replay");
        }
    }
}