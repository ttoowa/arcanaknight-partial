using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    [CreateAssetMenu(fileName = "GameChapter", menuName = "ScriptableObject/GameChapter", order = 1)]
    public class GameChapter : ScriptableObject, ILibraryData {
        public object Key => id;
        public string ContentCode => $"{ParentAct.ParentMode.name}.{ParentAct.id}.{id}";
        public bool IsPlayable => isUnlocked && !isWorkingContent;
        public GameAct ParentAct { get; private set; }

        public int id;
        public bool isWorkingContent;

        public bool isUnlocked;
        public GameDifficultyState difficultyState = new();
        public GameBalance gameBalance;
        public GameStageLibrary stageLibrary;

        public StoryClip endingStoryClip;

        public void Init(GameAct parentAct) {
            ParentAct = parentAct;
            difficultyState.Init();
            stageLibrary.Init();
        }

        public JObject ToJObject() {
            JObject jChapter = new();

            jChapter["id"] = id;
            jChapter["isUnlocked"] = isUnlocked;
            jChapter["DifficultyState"] = difficultyState.ToJObject();

            return jChapter;
        }

        public void LoadFromJObject(JObject jChapter) {
            if (jChapter == null) return;

            try {
                isUnlocked |= jChapter.TryGetValue<bool>("isUnlocked", isUnlocked);
                difficultyState.LoadFromJObject(jChapter.TryGetValue<JObject>("DifficultyState", null));
            } catch (Exception ex) {
                LogBuilder.Log(LogType.Error, "GameChapter.LoadFromJObject", $"Failed to load GameChapter.",
                    new[] { new LogElement("Exception", ex.ToString()) });
            }
        }

        public void UnlockNextChapter() {
            GameChapter nextChapter = ParentAct.chapterLibrary.GetData(id + 1);
            if (nextChapter == null) return;

            nextChapter.isUnlocked = true;

            SaveData.MarkAsDirty(true);
        }
    }
}