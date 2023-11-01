using System;
using ArcaneSurvivorsClient.Locale;
using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    [CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObject/Weapon", order = 1)]
    public class Weapon : ScriptableObject, ILibraryData {
        public object Key => (int)weaponType;

        public WeaponType weaponType;

        public GameObject[] prefabResources;

        [LocaleKey]
        public new string name;

        [LocaleKey]
        public string description;

        public SynergyType[] synergies;

        public WeaponStat[] weaponStats;

        public Sprite GetIcon(int level) {
            return weaponStats[GetStatIndex(level)].icon;
        }

        public WeaponStat GetStat(int level) {
            return weaponStats[GetStatIndex(level)];
        }

        public int GetStatIndex(int preferedLevel) {
            return Mathf.Clamp(preferedLevel - 1, 0, weaponStats.Length - 1);
        }
    }
}