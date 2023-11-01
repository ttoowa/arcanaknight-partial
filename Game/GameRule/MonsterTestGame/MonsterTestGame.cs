using System;
using Unity.VisualScripting;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class MonsterTestGame : MonoBehaviour, IGame {
        public string ContentCode => "MonsterTest";
        public bool IsPlaying => isPlaying;
        public bool IsBattlePhase => isPlaying;

        public GameRuleType RuleType => GameRuleType.MonsterTest;

        public GameChapter PlayingChapter { get; private set; }

        public float NormalizedStageNum => 0f;
        public bool IsFirstStage => true;
        public bool IsLastStage => true;
        public GameStage CurrentStage => currentStage;

        public GameStatistics Statistics { get; private set; }

        public float Gold { get; }
        public float ScoreProgress { get; }
        public float Score { get; }

        private GamePlayer localPlayer;
        private bool isPlaying;

        private int stageNum;

        private GameStage currentStage;

        public Transform[] playerTeleportSpots;

        private float teleportTimer;
        private int teleportIndex;
        private const float TeleportDelay = 3f;

        public event IGame.ScoreChangedDelegate ScoreProgressChanged;
        public event IGame.GoldAssetChangedDelegate GoldAssetChanged;
        public event IGame.PlayerHpChangedDelegate PlayerHpChanged;

        private void Start() {
            Statistics = new GameStatistics();
        }

        public void ResetGame() {
            //SetDay(1);
            SetGold(0);
            //SetPhase(GamePhase.Day);
        }

        public void StartGame(GameChapter chapter) {
            PlayingChapter = chapter;

            ResetGame();
            NextStage();

            currentStage.map.CreateInstance();

            WeaponInventory.Instance.Clear();
            localPlayer =
                GamePlayerManager.Instance.SpawnLocalPlayer(PlayableCharacterManager.Instance.SelectedCharacter);
            localPlayer.StatusUI.SetVisible(false);

            SetScore(0);
            SetMaxScore(1);

            // Test starting gold
            SetGold(10000);
        }

        public void EndGame() {
            if (localPlayer != null) {
                GamePlayerManager.Instance.RemoveLocalPlayer();
                localPlayer = null;
            }

            WeaponInventory.Instance.Clear();
            GamePlayerManager.Instance.RemoveLocalPlayer();

            currentStage.map.RemoveInstance();
        }

        public void GameOver() {
        }

        public void NextStage() {
            ++stageNum;
            currentStage = PlayingChapter.stageLibrary.GetData(stageNum);

            MonsterManager.Instance.RemoveAll();

            MonsterType[] monsterTypes = Enum.GetValues(typeof(MonsterType)) as MonsterType[];
            MonsterType monsterType = monsterTypes[Mathf.Clamp(stageNum + 1, 0, monsterTypes.Length - 1)];
            MonsterManager.Instance.SpawnMonster(monsterType);
        }

        public void OnTick(float deltaTime) {
            if (Input.GetKeyDown(KeyCode.N))
                NextStage();

            if (Input.GetKeyDown(KeyCode.T)) {
                teleportIndex = (teleportIndex + 1) % playerTeleportSpots.Length;
                localPlayer.Pawn.WorldPosition = playerTeleportSpots[teleportIndex].position;
            }
        }

        public void SetGold(float amount) {
        }

        public void AddGold(float amount) {
        }

        public bool SubtractGold(float amount) {
            return true;
        }

        public void SetMaxScore(float score) {
        }

        public void AddScore(float score) {
        }

        public void SetScore(float score) {
        }
    }
}