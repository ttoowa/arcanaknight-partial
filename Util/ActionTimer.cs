using System;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class ActionTimer : IDisposable, IPauseable {
        public float ElapsedSeconds { get; private set; }
        public bool IsRunning { get; private set; }

        public float targetSeconds;
        public event Action Action;

        public ActionTimer(float targetSeconds, bool autoUpdate = true) {
            this.targetSeconds = targetSeconds;

            if (autoUpdate)
                ActionTimerDispatcher.Instance.AddTimer(this);
        }

        public void Dispose() {
            ActionTimerDispatcher.Instance.RemoveTimer(this);
        }

        public void SetRunning(bool isRunning) {
            IsRunning = isRunning;
        }

        public void Update(float deltaTime) {
            if (!IsRunning) return;

            ElapsedSeconds += deltaTime;
            if (ElapsedSeconds >= targetSeconds) {
                Action?.Invoke();
                ElapsedSeconds -= targetSeconds;
            }
        }
    }
}