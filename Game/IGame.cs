using System;
using ArcaneSurvivorsClient.Game;

namespace ArcaneSurvivorsClient.Game {
    public interface IGame {
        public struct ScoreChangedInfo {
            public float score;
            public float maxScore;
            public float progress;
        }

        public delegate void ScoreChangedDelegate(ScoreChangedInfo info);

        public delegate void GoldAssetChangedDelegate(float oldAmount, float newAmount);

        public delegate void PlayerHpChangedDelegate(float hp, float maxHp);

        public string ContentCode { get; }
        public bool IsPlaying { get; }
        public bool IsBattlePhase { get; }

        public GameRuleType RuleType { get; }
        public GameChapter PlayingChapter { get; }

        public float NormalizedStageNum { get; }
        public bool IsFirstStage { get; }
        public bool IsLastStage { get; }
        public GameStage CurrentStage { get; }

        public GameStatistics Statistics { get; }

        public float Gold { get; }

        public float ScoreProgress { get; }

        public float Score { get; }

        public event ScoreChangedDelegate ScoreProgressChanged;
        public event GoldAssetChangedDelegate GoldAssetChanged;
        public event PlayerHpChangedDelegate PlayerHpChanged;

        // Phase & Day
        // 캐릭터 및 맵을 세팅하고 게임을 시작하는 함수
        public void StartGame(GameChapter chapter);

        // 캐릭터가 죽거나 클리어 후 게임의 구성요소를 제거하는 함수
        public void EndGame();

        public void GameOver();

        public void NextStage();

        public void OnTick(float deltaTime);

        // Asset
        public void SetGold(float amount);

        public void AddGold(float amount);

        public bool SubtractGold(float amount);

        // Progress
        public void SetMaxScore(float score);
        public void AddScore(float score);
        public void SetScore(float score);
    }
}