using System;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    [CreateAssetMenu(fileName = "GameStage", menuName = "ScriptableObject/GameStage", order = 1)]
    public class GameStage : ScriptableObject, ILibraryData {
        public object Key => stageNum;
        public string Name => $"Stage {stageNum}";

        public int stageNum;
        public GameReward reward;
        public StageMap map;

        public MonsterType[] normalMonsterPool;
        public MonsterType[] eventMonsterPool;
        public MonsterType bossMonster;
    }
}