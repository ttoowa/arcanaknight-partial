using System;
using ArcaneSurvivorsClient.Locale;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    public struct GameDifficultyEffect {
        [HideInInspector]
        [LocaleKey]
        public string header;

        public StatValue statValue;
    }
}