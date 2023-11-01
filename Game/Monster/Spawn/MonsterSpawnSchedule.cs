using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class MonsterSpawnSchedule {
        public bool IsComplete => currentStep >= spawnStepList.Count;
        public float LastSpawnTime => spawnStepList.Count > 0 ? spawnStepList[spawnStepList.Count - 1].time : 0;
        public IReadOnlyList<MonsterSpawnStep> SpawnStepList => spawnStepList;

        public int CurrentStep => currentStep;


        private readonly List<MonsterSpawnStep> spawnStepList = new();
        private int currentStep;

        public MonsterSpawnSchedule() {
        }

        public override string ToString() {
            StringBuilder builder = new();
            foreach (MonsterSpawnStep step in spawnStepList) {
                builder.AppendLine(step.ToString());
            }

            return builder.ToString();
        }

        public MonsterSpawnStep? GetSpawnStep(float currentTime) {
            if (currentStep >= spawnStepList.Count)
                return null;

            MonsterSpawnStep spawnStep = spawnStepList[currentStep];
            if (spawnStep.time <= currentTime) {
                ++currentStep;
                return spawnStep;
            }

            return null;
        }

        public float GoToMiddle() {
            if (spawnStepList.Count == 0) return 0f;

            currentStep = spawnStepList.Count / 2;
            MonsterSpawnStep middleStep = spawnStepList[currentStep];

            return middleStep.time;
        }

        public void Clear() {
            spawnStepList.Clear();
        }

        public void AddStep(MonsterSpawnStep spawnStep) {
            spawnStepList.Add(spawnStep);
        }

        public void RemoveStep(MonsterSpawnStep spawnStep) {
            spawnStepList.Remove(spawnStep);
        }
    }
}