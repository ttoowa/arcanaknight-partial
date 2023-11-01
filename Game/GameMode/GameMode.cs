using System;
using ArcaneSurvivorsClient.Locale;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    [CreateAssetMenu(fileName = "GameMode", menuName = "ScriptableObject/GameMode", order = 1)]
    public class GameMode : ScriptableObject, ILibraryData {
        public object Key => name;

        public bool isAvailable = true;

        [LocaleKey]
        public string name;

        [LocaleKey]
        public string desc;

        public Sprite thumbnailSprite;

        public GameActLibrary actLibrary;

        public bool autoSelectFirstAct;
        public bool autoSelectFirstChapter;

        public void Init() {
            actLibrary.Init(this);
        }

        public JObject ToJObject() {
            JObject jMode = new();
            jMode["name"] = name;

            JArray jActs = new();
            jMode.Add("Acts", jActs);

            foreach (GameAct act in actLibrary.dataObjects) {
                jActs.Add(act.ToJObject());
            }

            return jMode;
        }

        public void LoadFromJObject(JObject jMode) {
            if (jMode == null) return;

            try {
                JArray jActs = jMode.TryGetValue<JArray>("Acts", null);
                foreach (JObject jAct in jActs) {
                    int id = jAct.TryGetValue<int>("id", -1);
                    if (id < 0) continue;

                    GameAct act = actLibrary.GetData(id);
                    if (act == null) continue;

                    act.LoadFromJObject(jAct);
                }
            } catch (Exception ex) {
                LogBuilder.Log(LogType.Error, "GameMode.LoadFromJObject", $"Failed to load GameMode.",
                    new[] { new LogElement("Exception", ex.ToString()) });
            }
        }
    }
}