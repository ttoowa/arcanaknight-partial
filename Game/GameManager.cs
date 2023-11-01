using System;
using System.Collections.Generic;
using ArcaneSurvivorsClient.Analytics;
using Firebase.Analytics;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class GameManager : MonoBehaviour, IPauseable {
        public delegate void GameStartedDelegate(IGame game);

        public static GameManager Instance { get; private set; }
        public IGame PlayingGame { get; private set; }

        public bool IsPlaying => PlayingGame != null;
        public bool IsPlayedOnSession { get; private set; }

        // Inspector fields
        public StandardGame standardGame;
        public MonsterTestGame monsterTestGame;

        private readonly List<GameObject> gameObjectList = new();

        // Selector fields
        [HideInInspector]
        public GameChapter selectedChapter;


        public event GameStartedDelegate PreGameStarted;
        public event GameStartedDelegate GameStarted;
        public event GameStartedDelegate GameEnded;

        private void Awake() {
            Instance = this;
        }

        private void Update() {
            if (PlayingGame != null) {
                PlayingGame.OnTick(Time.deltaTime);

                if (GamePlayer.LocalPlayer != null && GamePlayer.LocalPlayer.Pawn.IsAlive)
                    PlayingGame.Statistics.playtime += TimeSpan.FromSeconds(Time.deltaTime);
            }

            bool isPlayerAlive = GamePlayer.LocalPlayer != null && GamePlayer.LocalPlayer.Pawn.IsAlive;
            PostProcessManager.Instance.SetSaturate(isPlayerAlive ? 0f : -100f);
        }

        public void StartGame(GameChapter chapter) {
            PlayingGame = standardGame;
            // switch (gameType) {
            //     case GameRuleType.Standard:
            //         PlayingGame = standardGame;
            //         break;
            //     case GameRuleType.MonsterTest:
            //         PlayingGame = monsterTestGame;
            //         break;
            // }
            selectedChapter = chapter;

            PreGameStarted?.Invoke(PlayingGame);
            PlayingGame.StartGame(selectedChapter);
            GameStarted?.Invoke(PlayingGame);

            IsPlayedOnSession = true;

            GameAnalytics.LogEvent("SceneEntered", new Parameter("name", "GameScene"));
        }

        public void EndGame() {
            PlayingGame.EndGame();
            GameEnded?.Invoke(PlayingGame);

            PlayingGame = null;
        }

        public void EndGameAndReturnToLobby() {
            EndGame();
            MainMenuUI.Instance.SetVisible(true);
        }

        public void RestartGame() {
            EndGame();
            StartGame(selectedChapter);
        }


        public void AddManagedGameObject(GameObject gameObject) {
            gameObjectList.Add(gameObject);
        }

        public void ClearGameObjects() {
            GameObject[] gameObjects = gameObjectList.ToArray();
            foreach (GameObject gameObject in gameObjects) {
                Destroy(gameObject);
            }

            gameObjectList.Clear();
        }
    }
}