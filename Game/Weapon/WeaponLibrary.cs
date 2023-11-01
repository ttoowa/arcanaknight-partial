using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [CreateAssetMenu(fileName = "WeaponLibrary", menuName = "ScriptableObject/WeaponLibrary", order = 1)]
    public class WeaponLibrary : DataLibrary<Weapon, WeaponType> {
    }
}