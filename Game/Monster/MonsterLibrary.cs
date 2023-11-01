using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient.Game {
    [CreateAssetMenu(fileName = "MonsterLibrary", menuName = "ScriptableObject/MonsterLibrary", order = 1)]
    public class MonsterLibrary : DataLibrary<Monster, MonsterType> {
    }
}