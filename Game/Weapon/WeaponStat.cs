using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    public class WeaponStat {
        public Sprite icon;
        public bool isCloseWeapon;
        public float cooltimeFactor = 1f;
        public float damageFactor = 1f;
        public float knockbackFactor = 1f;
    }
}