using ArcaneSurvivorsClient.Game;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Script.Develop.GameCheat {
    public class GameCheat : MonoBehaviour {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        public Button skipPhaseButton;

        public Button skipDayButton;

        public Button unlockDifficultyButton;

        private void Start() {
            skipPhaseButton.onClick.AddListener(() => {
                if (!GameManager.Instance.IsPlaying) return;

                if (GameManager.Instance.standardGame.IsPlaying)
                    GameManager.Instance.standardGame.SkipNightPhase();
            });

            skipDayButton.onClick.AddListener(() => {
                if (!GameManager.Instance.IsPlaying) return;

                if (GameManager.Instance.standardGame.IsPlaying)
                    GameManager.Instance.standardGame.AddStage();
            });

            unlockDifficultyButton.onClick.AddListener(() => {
                if (GameModeSelector.Instance.SelectedChapter == null) return;

                GameModeSelector.Instance.SelectedChapter.difficultyState.UnlockPlayableDifficultyLevel(6);
            });
        }
#endif
    }
}