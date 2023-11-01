using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class Weapon_GoldenFork : RuntimeWeaponBase {
        public override void OnEquip() {
        }

        public override void OnDestroy() {
        }

        public override void OnRepeatWeaponTick(float cooltime) {
            switch (OwnerWeaponBundle.level) {
                case 1: {
                    EmitProjectiles(1, 2);
                    break;
                }
                case 2: {
                    EmitProjectiles(4, 3);
                    break;
                }
                case 3: {
                    EmitProjectiles(6, 5);
                    break;
                }
            }
        }

        public override void OnTick(float deltaTime) {
        }

        public override void OnHit(Pawn pawn) {
            //SfxPlayer.Play("weapon.sword.hit", true);
        }

        public override void OnLevelChanged(int level) {
        }

        private void EmitProjectiles(int count, int penetration) {
            for (int i = 0; i < count; ++i) {
                GameObject projectile = OwnerWeaponBundle.weapon.prefabResources[0]
                    .InstantiateFX(OwnerPawn.FXPosition, false, 2f);
                projectile.InitPlayerHitboxes(OwnerPawn, this, (hitbox) => {
                    hitbox.penetration = penetration;
                });
            }

            //SfxPlayer.Play("weapon.sword.swing", true);
        }
    }
}