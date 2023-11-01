using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    public class PawnAbility : ICloneable {
        public string DisplayDamageScale => $"{(damageScale * 100f).ToString("0.0")}%";
        public string DisplayAttackCooltimeScale => $"{(attackCooltimeScale * 100f).ToString("0.0")}%";
        public string DisplayMoveSpeedScale => $"{(moveSpeedScale * 100f).ToString("0.0")}%";

        public float hp = 100f;
        public float hpRegen = 0f;

        public float damageScale = 1f;
        public float attackCooltimeScale = 1f;

        public float moveSpeedScale = 1f;

        [Range(0f, 0.9f)]
        public float friction = 0.3f;

        public object Clone() {
            return (PawnAbility)MemberwiseClone();
        }
    }
}