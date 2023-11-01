using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient.Game {
    [CreateAssetMenu(fileName = "SynergyLibrary", menuName = "ScriptableObject/SynergyLibrary", order = 1)]
    public class SynergyLibrary : DataLibrary<Synergy, SynergyType> {
    }
}