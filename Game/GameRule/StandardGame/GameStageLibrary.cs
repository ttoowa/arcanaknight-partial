using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient.Game {
    [CreateAssetMenu(fileName = "DayStageLibrary", menuName = "ScriptableObject/DayStageLibrary", order = 1)]
    public class GameStageLibrary : DataLibrary<GameStage, int> {
        public void Init() {
            Indexing();
        }
    }
}