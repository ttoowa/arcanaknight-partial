using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public static class MonsterSpawnLogic {
        public delegate void ForeachSpawnPoolDelegate(MonsterType monsterType, int index);

        public static MonsterSpawnSchedule GenerateSpawnSchedule(MonsterSpawnParams spawnParams) {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                MonsterResource.Instance.library.Indexing();
#endif

            MonsterSpawnSchedule schedule = new();
            MonsterSpawnSet[] spawnSets = GenerateSpawnSets(spawnParams.spawnPool, spawnParams.maxSpawnSetElementCount);

            float scheduleHpSum = 0;

            float partialHpSum = (float)spawnParams.monsterHpSum / spawnParams.spawnStep;
            float partialTime = spawnParams.stageDuration / spawnParams.spawnStep;
            for (int step = 0; step < spawnParams.spawnStep; ++step) {
                float normalizedStep = (float)step / spawnParams.spawnStep;
                float targetHpSum = partialHpSum * ((float)step / (spawnParams.spawnStep - 1) + 0.5f);

                MonsterSpawnSet spawnSet = spawnSets[Mathf.FloorToInt(normalizedStep * spawnSets.Length)];
                int spawnCount = Mathf.CeilToInt(targetHpSum / spawnSet.HpSum);

                for (int spawnI = 0; spawnI < spawnCount; ++spawnI) {
                    float normalizedSpawnI = (float)spawnI / spawnCount;
                    float time = spawnParams.stageDuration * normalizedStep + partialTime * normalizedSpawnI;

                    scheduleHpSum += spawnSet.HpSum;
                    schedule.AddStep(new MonsterSpawnStep(time, spawnSet));
                }
            }

            float hpError = scheduleHpSum - spawnParams.monsterHpSum;

            MonsterSpawnStep? errorNearestStep = null;
            float errorNearestStepDistance = float.MaxValue;
            foreach (MonsterSpawnStep step in schedule.SpawnStepList) {
                float distance = hpError - step.HpSum;
                if (step.HpSum < hpError && errorNearestStepDistance > distance) {
                    errorNearestStep = step;
                    errorNearestStepDistance = distance;
                }
            }

            if (errorNearestStep.HasValue)
                schedule.RemoveStep(errorNearestStep.Value);

            Debug.Log(schedule.ToString());

            return schedule;
        }

        private static MonsterSpawnSet[] GenerateSpawnSets(MonsterType[] spawnPool, int maxElementCount) {
            if (maxElementCount < 1) {
                throw LogBuilder.BuildException(LogType.Error, nameof(MonsterSpawnLogic),
                    "elementCount must be greater than 0",
                    new LogElement(nameof(maxElementCount), maxElementCount.ToString()));
            }

            List<MonsterType[]> spawnSetRawList = new();

            for (int i = 1; i <= maxElementCount; ++i) {
                MonsterType[] set = new MonsterType[i];
                GenerateSpawnSetRecursive(spawnPool, spawnSetRawList, set, 0, 0, i);
            }

            List<MonsterSpawnSet> spawnSetList = spawnSetRawList.Select(x =>
                new MonsterSpawnSet(x.Select(y => MonsterResource.Instance.library.GetData(y)).ToArray())).ToList();

            spawnSetList.Sort((left, right) => {
                double leftHp = left.HpSum;
                double rightHp = right.HpSum;

                return leftHp.CompareTo(rightHp);
            });

            return spawnSetList.ToArray();
        }

        private static void GenerateSpawnSetRecursive(MonsterType[] spawnPool, List<MonsterType[]> spawnSetList,
            MonsterType[] set, int depth, int startIndex, int elementCount) {
            ForeachSpawnPool(spawnPool, (monsterType, index) => {
                set[depth] = monsterType;

                if (depth == elementCount - 1) {
                    // Last depth
                    spawnSetList.Add(set.ToArray());
                } else
                    GenerateSpawnSetRecursive(spawnPool, spawnSetList, set, depth + 1, index + 1, elementCount);
            }, startIndex, Mathf.Min(spawnPool.Length, spawnPool.Length - elementCount + depth + 1));
        }

        private static void ForeachSpawnPool(MonsterType[] spawnPool, ForeachSpawnPoolDelegate action, int startIndex,
            int length) {
            for (int i = startIndex; i < length; ++i) {
                action(spawnPool[i], i);
            }
        }
    }
}