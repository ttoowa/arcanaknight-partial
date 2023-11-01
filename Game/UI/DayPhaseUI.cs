using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient.Game {
    public class DayPhaseUI : MonoBehaviour {
        public GameObject panel;

        public GameObject[] extraContents;

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
                    break;
            }
        }

        private void OnPhaseChanged(GamePhase gamePhase, StandardGame.StageInfo stage) {
            SetVisible(gamePhase == GamePhase.Day);
        }


        private void OnGameEnded(IGame game) {
            switch (game.RuleType) {
                case GameRuleType.Standard:
                    StandardGame standardGame = game as StandardGame;
                    standardGame.PhaseChanged -= OnPhaseChanged;
                    break;
            }
        }
    }
}