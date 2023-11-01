using System;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    [CreateAssetMenu(fileName = "GameModeLibrary", menuName = "ScriptableObject/GameModeLibrary", order = 1)]
    public class GameModeLibrary : DataLibrary<GameMode, string> {
        public void Init() {
            Indexing();
            foreach (GameMode mode in dataObjects) {
                mode.Init();
            }
        }
    }
}