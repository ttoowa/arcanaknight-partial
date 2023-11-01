using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [CreateAssetMenu(fileName = "GameDifficultyLibrary", menuName = "ScriptableObject/GameDifficultyLibrary",
        order = 1)]
    public class GameDifficultyLibrary : DataLibrary<GameDifficulty, int> {
        private bool isInitialized;

        public void Init() {
            Indexing();

            foreach (GameDifficulty difficulty in dataObjects) {
                difficulty.Init();
            }
        }
    }
}