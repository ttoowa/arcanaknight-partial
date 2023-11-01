using System.Collections.Generic;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class ActionTimerDispatcher : MonoBehaviour, ISingletone, IPauseable {
        public static ActionTimerDispatcher Instance { get; private set; }

        private List<ActionTimer> timerList;

        private void Awake() {
            Instance = this;

            timerList = new List<ActionTimer>();
        }

        private void Update() {
            ActionTimer[] timers = timerList.ToArray();
            foreach (ActionTimer timer in timers) {
                if (timer == null) {
                    RemoveTimer(timer);
                    continue;
                }

                if (!timer.IsRunning)
                    continue;

                timer.Update(Time.deltaTime);
            }
        }

        public void Clear() {
            timerList.Clear();
        }

        public void AddTimer(ActionTimer timer) {
            timerList.Add(timer);
        }

        public bool RemoveTimer(ActionTimer timer) {
            return timerList.Remove(timer);
        }
    }
}