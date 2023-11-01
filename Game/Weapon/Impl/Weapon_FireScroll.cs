using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class Weapon_FireScroll : RuntimeWeaponBase {
        public override void OnEquip() {
        }

        public override void OnDestroy() {
        }

        public override void OnRepeatWeaponTick(float cooltime) {
            if (MonsterManager.Instance.NearestMonster == null) return;

            GameObject fireball = OwnerWeaponBundle.weapon.prefabResources[0]
                .InstantiateFX(OwnerPawn.FXPosition, false);
            fireball.transform.localEulerAngles = new Vector3(0f, OwnerPawn.CurrentAngle, 0f);
            fireball.InitPlayerHitboxes(OwnerPawn, this);
            fireball.GetComponent<ThrowingObject>().Normal = MonsterManager.Instance.NearestMonsterDirection;

            SfxPlayer.Play("weapon.fire.fire", true);
        }

        public override void OnTick(float deltaTime) {
        }

        public override void OnHit(Pawn pawn) {
            SfxPlayer.Play("weapon.fire.hit", true);
        }

        public override void OnLevelChanged(int level) {
        }
    }
}