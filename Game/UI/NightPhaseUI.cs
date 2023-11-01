using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient.Game {
    public class NightPhaseUI : MonoBehaviour {
        public GameObject panel;
        public GameObject[] extraContents;

        public TimerText gameTimer;

        private void Start() {
            SetVisible(false);

            GameManager.Instance.PreGameStarted += OnPreGameStarted;
            GameManager.Instance.GameEnded += OnGameEnded;
        }

        public void SetVisible(bool visible) {
            panel.SetActive(visible);

            extraContents.ForEach(content => {
                if (content != null)
                    content.SetActive(visible);
            });
        }

        private void OnPreGameStarted(IGame game) {
            switch (game.RuleType) {
                case GameRuleType.Standard:
                    StandardGame standardGame = game as StandardGame;
                    standardGame.PhaseChanged += OnPhaseChanged;
                    standardGame.DayChanged += OnDayChanged;
                    standardGame.ScoreProgressChanged += OnScoreProgressChanged;
                    break;
            }
        }

        private void OnPhaseChanged(GamePhase gamePhase, StandardGame.StageInfo stage) {
            SetVisible(gamePhase == GamePhase.Night || gamePhase == GamePhase.NightResult);
        }


        private void OnGameEnded(IGame game) {
            switch (game.RuleType) {
                case GameRuleType.Standard:
                    StandardGame standardGame = game as StandardGame;
                    standardGame.PhaseChanged -= OnPhaseChanged;
                    standardGame.DayChanged -= OnDayChanged;
                    standardGame.ScoreProgressChanged -= OnScoreProgressChanged;
                    break;
            }
        }

        private void OnScoreProgressChanged(IGame.ScoreChangedInfo info) {
        }

        private void OnDayChanged(StandardGame.StageInfo stageInfo) {
        }
    }
}