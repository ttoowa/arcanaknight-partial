using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class JobInvoker : MonoBehaviour {
        public class Job {
            public Action action;
            public float targetTime;
            public float elapsedTime;

            public Job(Action action, float targetTime) {
                this.action = action;
                this.targetTime = targetTime;
            }
        }

        public static JobInvoker Instance { get; private set; }

        private List<Job> jobList = new();

        public void AddJob(Action action, float? targetTime = null) {
            jobList.Add(new Job(action, targetTime.HasValue ? targetTime.Value : 0f));
        }

        private void Awake() {
            Instance = this;
        }

        private void Update() {
            for (int i = 0; i < jobList.Count; i++) {
                Job job = jobList[i];
                job.elapsedTime += Time.deltaTime;
                if (job.elapsedTime >= job.targetTime) {
                    job.action();
                    jobList[i] = null;
                }
            }

            jobList.RemoveAll(job => job == null);
        }
    }
}