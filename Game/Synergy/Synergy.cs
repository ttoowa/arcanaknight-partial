using System;
using ArcaneSurvivorsClient.Locale;
using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    [CreateAssetMenu(fileName = "Synergy", menuName = "ScriptableObject/Synergy", order = 1)]
    public class Synergy : ScriptableObject, ILibraryData {
        public object Key => (int)synergyType;

        public int MaxLevel {
            get {
                int maxLevel = 0;
                foreach (SynergyBuffSpec buffSpec in buffSpecs) {
                    maxLevel = Mathf.Max(maxLevel, buffSpec.conditionLevel);
                }

                return maxLevel;
            }
        }

        public SynergyType synergyType;

        public Sprite icon;
        public Sprite buffIcon;

        public SynergyBuffSpec[] buffSpecs;

        [LocaleKey] public new string name;

        [LocaleKey] public string description;

        [LocaleKey] public string buffName;

        [LocaleKey] public string buffDescription;

#if UNITY_EDITOR
        [Multiline] public string developNote;
#endif
    }
}