using System;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    public struct SynergyBuffSpec {
        public int conditionLevel;
        public StatValue[] buffValues;
    }
}