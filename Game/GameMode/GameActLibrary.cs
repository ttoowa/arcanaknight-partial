using System;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    [CreateAssetMenu(fileName = "GameActLibrary", menuName = "ScriptableObject/GameActLibrary", order = 1)]
    public class GameActLibrary : DataLibrary<GameAct, int> {
        public void Init(GameMode parentMode) {
            Indexing();
            foreach (GameAct act in dataObjects) {
                act.Init(parentMode);
            }
        }
    }
}