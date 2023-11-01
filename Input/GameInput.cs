using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ArcaneSurvivorsClient {
    public interface IGameInput {
        void UpdateAndRun();
    }

    public class GameInput : IGameInput {
        public event Action action;
        public readonly List<GameInputTrigger> triggerList;
        public readonly GameInputRate rate;

        private bool prevState;

        public GameInput(GameInputRate rate) {
            triggerList = new List<GameInputTrigger>();
            this.rate = rate;
        }

        public void UpdateAndRun() {
            bool currentState = CheckCondition();


            bool invokeFlag = false;
            switch (rate) {
                case GameInputRate.TriggerEnter:
                    if (!prevState && currentState) { invokeFlag = true; }

                    break;
                case GameInputRate.TriggerHold:
                    if (currentState) { invokeFlag = true; }

                    break;
                case GameInputRate.TriggerExit:
                    if (prevState && !currentState) { invokeFlag = true; }

                    break;
            }

            if (invokeFlag) { action?.Invoke(); }

            prevState = currentState;
        }

        private bool CheckCondition() {
            foreach (GameInputTrigger trigger in triggerList) {
                if (trigger.Check()) { return true; }
            }

            return false;
        }
    }

    public class GameInput<T> : IGameInput {

        public event Action<T> action;
        public readonly List<GameInputTrigger<T>> triggerList;
        public readonly GameInputRate rate;

        private bool prevState;

        public GameInput(GameInputRate rate) {
            triggerList = new List<GameInputTrigger<T>>();
            this.rate = rate;
        }

        public void UpdateAndRun() {
            Tuple<bool, T> triggerResult = CheckCondition();
            bool currentState = triggerResult.Item1;

            bool invokeFlag = false;
            switch (rate) {
                case GameInputRate.TriggerEnter:
                    if (!prevState && currentState) { invokeFlag = true; }

                    break;
                case GameInputRate.TriggerHold:
                    if (currentState) { invokeFlag = true; }

                    break;
                case GameInputRate.TriggerExit:
                    if (prevState && !currentState) { invokeFlag = true; }

                    break;
            }

            if (invokeFlag) { action?.Invoke(triggerResult.Item2); }

            prevState = currentState;
        }

        private Tuple<bool, T> CheckCondition() {
            foreach (GameInputTrigger<T> trigger in triggerList) {
                Tuple<bool, T> result = trigger.Check();

                if (result.Item1) { return result; }
            }

            return new Tuple<bool, T>(false, default);
        }
    }
}