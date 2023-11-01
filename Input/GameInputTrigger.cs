using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class GameInputTrigger {
        private KeyCode? key;
        private List<ReturnDelegate<bool>> conditionList;

        public GameInputTrigger(KeyCode? key, params ReturnDelegate<bool>[] conditions) {
            conditionList = new List<ReturnDelegate<bool>>();

            this.key = key;
            conditionList.AddRange(conditions);
        }

        public bool Check() {
            if (key != null && !Input.GetKey((KeyCode)key)) { return false; }

            foreach (ReturnDelegate<bool> condition in conditionList) {
                if (!condition()) { return false; }
            }

            return true;
        }
    }

    public class GameInputTrigger<T> {
        private KeyCode? key;
        private ReturnDelegate<Tuple<bool, T>> mainCondition;
        private List<ReturnDelegate<bool>> subConditionList;

        public GameInputTrigger(KeyCode? key, ReturnDelegate<Tuple<bool, T>> mainCondition,
            params ReturnDelegate<bool>[] subConditions) {
            this.key = key;
            this.mainCondition = mainCondition;
            subConditionList = new List<ReturnDelegate<bool>>();
            subConditionList.AddRange(subConditions);
        }

        public Tuple<bool, T> Check() {
            if (key != null && !Input.GetKey((KeyCode)key)) { return new Tuple<bool, T>(false, default); }

            foreach (ReturnDelegate<bool> subCondition in subConditionList) {
                if (!subCondition()) { return new Tuple<bool, T>(false, default); }
            }

            Tuple<bool, T> mainConditionResult = mainCondition();
            if (!mainConditionResult.Item1) { return new Tuple<bool, T>(false, default); }

            return mainConditionResult;
        }
    }
}