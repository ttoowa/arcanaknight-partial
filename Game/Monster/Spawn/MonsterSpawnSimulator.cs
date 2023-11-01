using System;
using System.Text;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class MonsterSpawnSimulator : MonoBehaviour {
#if UNITY_EDITOR
        public MonsterSpawnParams spawnParams = new() {
            maxSpawnSetElementCount = 3,
            monsterHpSum = 1000,
            spawnPool = new[] { MonsterType.GreenSlime, MonsterType.RedSlime, MonsterType.SpikySlime, MonsterType.Bat },
            spawnStep = 5,
            stageDuration = 120
        };

        public void Simulate() {
            if (MonsterResource.Instance == null) {
                Debug.LogError("MonsterResource.Instance is null.");
                return;
            }

            MonsterResource.Instance.library.Indexing();
            MonsterSpawnSchedule schedule = MonsterSpawnLogic.GenerateSpawnSchedule(spawnParams);

            float hpSum = 0f;
            int monsterCount = 0;
            foreach (MonsterSpawnStep spawnStep in schedule.SpawnStepList) {
                foreach (Monster monster in spawnStep.spawnSet.monsters) {
                    hpSum += monster.SpawnHpSum;
                    ++monsterCount;
                }
            }

            StringBuilder builder = new();
            builder.AppendLine($"[몬스터 스폰 시뮬레이션]\n" + schedule.ToString());
            builder.AppendLine();
            builder.AppendLine($"몬스터 수 : {monsterCount}");
            builder.AppendLine(
                $"HP 총합 : {hpSum}, 목표 HP 총합 : {spawnParams.monsterHpSum} (오차 : {hpSum - spawnParams.monsterHpSum})");

            Debug.Log(builder.ToString());
        }
#endif
    }
}