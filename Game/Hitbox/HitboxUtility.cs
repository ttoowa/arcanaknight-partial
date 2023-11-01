using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public delegate void HitboxDelegate(Hitbox hitbox);

    public static class HitboxUtility {
        public static void InitHitboxes(this GameObject hitboxParent, Pawn ownerPawn, RuntimeWeaponBase ownerWeapon,
            HitboxDelegate hitboxProcess = null) {
            hitboxParent.ForeachChildComponents<Hitbox>((Hitbox hitbox) => {
                hitbox.ownerPawn = ownerPawn;
                hitbox.ownerWeapon = ownerWeapon;

                hitboxProcess?.Invoke(hitbox);
            });
        }

        public static void InitPlayerHitboxes(this GameObject hitboxParent, Pawn ownerPawn,
            RuntimeWeaponBase ownerWeapon, HitboxDelegate hitboxProcess = null) {
            GameBalance balance = GameManager.Instance.PlayingGame.PlayingChapter.gameBalance;

            bool isCloseAttack = ownerWeapon.OwnerWeaponBundle.ActualStat.isCloseWeapon;
            float actualDamage = ownerPawn.GetActualDamage(ownerWeapon);

            hitboxProcess += (hitbox) => {
                hitbox.isCloseAttack = isCloseAttack;
                hitbox.damage = actualDamage;
            };

            InitHitboxes(hitboxParent, ownerPawn, ownerWeapon, hitboxProcess);
        }

        public static void InitMonsterHitboxes(this GameObject hitboxParent, Pawn ownerPawn,
            HitboxDelegate hitboxProcess = null) {
            GameBalance balance = GameManager.Instance.PlayingGame.PlayingChapter.gameBalance;

            float damage = StatCalculator.GetDamage(ownerPawn.force, ownerPawn.BaseDamage, null);

            hitboxProcess += (hitbox) => {
                hitbox.isCloseAttack = true;
                hitbox.damage = damage;
            };

            InitHitboxes(hitboxParent, ownerPawn, null, hitboxProcess);
        }
    }
}