using System;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    [CreateAssetMenu(fileName = "UpgradableStatLibrary", menuName = "ScriptableObject/UpgradableStatLibrary",
        order = 1)]
    public class UpgradableStatLibrary : DataLibrary<UpgradableStat, UpgradableStatType> {
    }
}