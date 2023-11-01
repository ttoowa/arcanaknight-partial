using System;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    public struct MonsterSpawnParams {
        public MonsterType[] spawnPool;

        [Range(1, 4)]
        public int maxSpawnSetElementCount;

        public int spawnStep;
        public int monsterHpSum;
        public float stageDuration;
    }
}