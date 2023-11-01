using System;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    [CreateAssetMenu(fileName = "StoryClip", menuName = "ScriptableObject/StoryClip", order = 1)]
    public class StoryClip : ScriptableObject {
        public StoryFrame[] frames;
    }
}