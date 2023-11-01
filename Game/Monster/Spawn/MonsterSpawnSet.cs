using System.Linq;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public struct MonsterSpawnSet {
        public readonly Monster[] monsters;
        public readonly float HpSum;

        public MonsterSpawnSet(params Monster[] monsters) {
            this.monsters = monsters;

            HpSum = (float)monsters.Sum(x => x.SpawnHpSum);
        }

        public override string ToString() {
            return $"({string.Join(", ", monsters.Select(x => x.ToString()))})";
        }
    }
}