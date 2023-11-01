using System;
using ArcaneSurvivorsClient.Locale;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    public struct StoryFrame {
        public Sprite illustSprite;

        [LocaleKey]
        public string scriptContent;
    }
}