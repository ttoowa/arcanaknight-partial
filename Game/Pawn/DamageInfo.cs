using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class DamageInfo {
        public bool IsExpired => duration >= 0f && Time.timeSinceLevelLoad - firedTime >= duration;

        public float Damage => damage;
        public int DamageInt => Mathf.FloorToInt(Damage);

        public Pawn sourcePawn;
        public RuntimeWeaponBase sourceWeapon;
        public string hitboxId;
        public float damage;
        public float firedTime;
        public float duration;

        public DamageInfo(Pawn sourcePawn, RuntimeWeaponBase sourceWeapon, string hitboxId, float damage,
            float duration) {
            firedTime = Time.timeSinceLevelLoad;
            this.sourcePawn = sourcePawn;
            this.sourceWeapon = sourceWeapon;
            this.hitboxId = hitboxId;
            this.damage = damage;
            this.duration = duration;
        }
    }
}