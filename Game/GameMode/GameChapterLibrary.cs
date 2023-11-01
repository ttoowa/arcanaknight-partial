using System;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    [CreateAssetMenu(fileName = "GameChapterLibrary", menuName = "ScriptableObject/GameChapterLibrary", order = 1)]
    public class GameChapterLibrary : DataLibrary<GameChapter, int> {
        public void Init(GameAct parentAct) {
            Indexing();
            foreach (GameChapter chapter in dataObjects) {
                chapter.Init(parentAct);
            }
        }
    }
}