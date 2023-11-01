using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    [CreateAssetMenu(fileName = "Monster", menuName = "ScriptableObject/Monster", order = 1)]
    public class Monster : ScriptableObject, ILibraryData {
        public object Key => (int)monsterType;

        public MonsterType monsterType;
        public MonsterBrainType brainType;

        /// <summary>
        ///     몬스터의 HP와 한 번에 스폰되는 수를 곱한 값입니다.
        /// </summary>
        public int SpawnHpSum => (int)(ability.hp * spawnScale);

        public bool isFlying;

        public GameObject pawnPrefab;

        public int spawnScale = 1;

        public PawnAbility ability;


        public override string ToString() {
            return $"({monsterType} : {ability.hp} HP, {spawnScale} Spawn)";
        }
    }
}