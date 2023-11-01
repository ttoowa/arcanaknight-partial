using System;
using Newtonsoft.Json.Linq;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    public struct GameProgress {
        public int difficultyLevel;
        public int dayNum;

        public GameProgress(int difficultyLevel, int dayNum) {
            this.difficultyLevel = difficultyLevel;
            this.dayNum = dayNum;
        }

        public JObject ToJObject() {
            JObject jGameProgress = new();

            jGameProgress["difficultyLevel"] = difficultyLevel;
            jGameProgress["dayNum"] = dayNum;

            return jGameProgress;
        }

        public static GameProgress FromJObject(JObject jGameProgress) {
            GameProgress gameProgress = new();

            if (jGameProgress == null) return gameProgress;

            gameProgress.difficultyLevel = jGameProgress.TryGetValue<int>("difficultyLevel", 0);
            gameProgress.dayNum = jGameProgress.TryGetValue<int>("dayNum", 0);

            return gameProgress;
        }

        public static bool operator ==(GameProgress a, GameProgress b) {
            return a.difficultyLevel == b.difficultyLevel && a.dayNum == b.dayNum;
        }

        public static bool operator !=(GameProgress a, GameProgress b) {
            return a.difficultyLevel != b.difficultyLevel || a.dayNum != b.dayNum;
        }

        public static bool operator >(GameProgress a, GameProgress b) {
            return a.difficultyLevel > b.difficultyLevel ||
                   (a.difficultyLevel == b.difficultyLevel && a.dayNum > b.dayNum);
        }

        public static bool operator <(GameProgress a, GameProgress b) {
            return a.difficultyLevel < b.difficultyLevel ||
                   (a.difficultyLevel == b.difficultyLevel && a.dayNum < b.dayNum);
        }

        public static bool operator >=(GameProgress a, GameProgress b) {
            return a.difficultyLevel > b.difficultyLevel ||
                   (a.difficultyLevel == b.difficultyLevel && a.dayNum >= b.dayNum);
        }

        public static bool operator <=(GameProgress a, GameProgress b) {
            return a.difficultyLevel < b.difficultyLevel ||
                   (a.difficultyLevel == b.difficultyLevel && a.dayNum <= b.dayNum);
        }
    }
}