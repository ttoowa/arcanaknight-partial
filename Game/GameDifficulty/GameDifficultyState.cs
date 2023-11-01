using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public delegate void DifficultySelectedDelegate(GameDifficulty difficulty);

    public delegate void DifficultyUnlockedDelegate(int level);

    [Serializable]
    public class GameDifficultyState {
        public GameDifficultyLibrary library;

        public GameDifficulty SelectedDifficulty => selectedDifficulty;

        public int MaxDifficultyLevel =>
            library.dataObjects.Max(x => x.level);

        public int PlayableDifficultyLevel =>
            playableDifficultyLevel;

        private GameDifficulty selectedDifficulty;
        private int playableDifficultyLevel = 1;

        public event DifficultySelectedDelegate DifficultySelected;

        public event DifficultyUnlockedDelegate DifficultyUnlocked;

        public void Init() {
            library.Init();

            UnlockDefaultPlayableDifficultyLevel();
        }

        public JObject ToJObject() {
            JObject jDifficultyState = new();

            jDifficultyState["playableDifficultyLevel"] = playableDifficultyLevel;

            return jDifficultyState;
        }

        public void LoadFromJObject(JObject jDifficultyState) {
            if (jDifficultyState == null) return;

            playableDifficultyLevel = Mathf.Max(jDifficultyState.TryGetValue<int>("playableDifficultyLevel", 1),
                playableDifficultyLevel);
        }

        public void UnlockPlayableDifficultyLevel(int level) {
            if (PlayableDifficultyLevel >= level) return;

            playableDifficultyLevel = level;

            SaveData.MarkAsDirty();

            DifficultyUnlocked?.Invoke(level);
        }

        private void UnlockDefaultPlayableDifficultyLevel() {
            UnlockPlayableDifficultyLevel(2);
        }

        public void SelectDifficulty(int difficultyLevel) {
            SelectDifficulty(library.GetData(difficultyLevel));
        }

        public void SelectDifficulty(GameDifficulty difficulty) {
            selectedDifficulty = difficulty;

            DifficultySelected?.Invoke(difficulty);
        }
    }
}