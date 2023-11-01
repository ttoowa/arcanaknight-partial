using System;
using ArcaneSurvivorsClient.Locale;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    [CreateAssetMenu(fileName = "GameAct", menuName = "ScriptableObject/GameAct", order = 1)]
    public class GameAct : ScriptableObject, ILibraryData {
        public object Key => id;
        public string DisplayName => $"Act {id}.";

        public GameMode ParentMode { get; private set; }

        public int id;
        public bool isAvailable = true;

        [LocaleKey]
        public string title;

        public Sprite thumbnailSprite;

        public GameChapterLibrary chapterLibrary;

        public void Init(GameMode parentMode) {
            ParentMode = parentMode;

            chapterLibrary.Init(this);
        }

        public JObject ToJObject() {
            JObject jAct = new();

            jAct["id"] = id;

            JArray jChapters = new();
            jAct.Add("Chapters", jChapters);

            foreach (GameChapter chapter in chapterLibrary.dataObjects) {
                jChapters.Add(chapter.ToJObject());
            }

            return jAct;
        }

        public void LoadFromJObject(JObject jAct) {
            if (jAct == null) return;

            try {
                JArray jChapters = jAct.TryGetValue<JArray>("Chapters", null);
                foreach (JObject jChapter in jChapters) {
                    int id = jChapter.TryGetValue<int>("id", -1);
                    if (id < 0) continue;

                    GameChapter chapter = chapterLibrary.GetData(id);
                    if (chapter == null) continue;

                    chapter.LoadFromJObject(jChapter);
                }
            } catch (Exception ex) {
                LogBuilder.Log(LogType.Error, "GameAct.LoadFromJObject", $"Failed to load GameAct.",
                    new[] { new LogElement("Exception", ex.ToString()) });
            }
        }
    }
}