using System.Collections;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class Weapon_Dagger : RuntimeWeaponBase {
        public override void OnEquip() {
        }

        public override void OnDestroy() {
        }

        public override void OnRepeatWeaponTick(float cooltime) {
            float speed = 6f;
            switch (OwnerWeaponBundle.level) {
                case 1: {
                    FireDagger(OwnerWeaponBundle.weapon.prefabResources[0], 0f, 2, 0.15f);
                    break;
                }
                case 2: {
                    for (int i = 0; i < 3; ++i) {
                        FireDagger(OwnerWeaponBundle.weapon.prefabResources[0], -45f + 45f * i, 4, 0.04f);
                    }

                    break;
                }
                case 3: {
                    for (int i = 0; i < 8; ++i) {
                        FireDagger(OwnerWeaponBundle.weapon.prefabResources[1], 45f * i, 5, 0.04f);
                    }

                    break;
                }
            }
        }

        public override void OnTick(float deltaTime) {
        }

        public override void OnHit(Pawn pawn) {
            SfxPlayer.Play("weapon.sword.hit", true);
        }

        public override void OnLevelChanged(int level) {
        }

        private void FireDagger(GameObject projectilePrefab, float angleOffset, int hitCount, float hitDelay) {
            GameObject projectile = projectilePrefab.InstantiateFX(OwnerPawn.FootPosition, false, 3f);

            ThrowingObject throwingObject = projectile.Get<ThrowingObject>();
            throwingObject.Normal = OwnerPawn.TargetAngleNormalVec2.AddAngle(angleOffset)
                .ToVector3(VectorUtility.Vector2ToVector3.XYtoXZ);

            UnityEventListener eventListener = projectile.GetOrAdd<UnityEventListener>();
            eventListener.Events.OnTriggerEnter += (other) => {
                if (other.attachedRigidbody == null) return;
                Pawn pawn = other.attachedRigidbody.Get<Pawn>();
                if (pawn == null || pawn.force != PawnForce.Monster) return;

                projectile.Destroy();

                CoroutineDispatcher.Dispatch(HitRoutine(pawn, hitCount, hitDelay));
            };

            //SfxPlayer.Play("weapon.sword.swing", true);
        }

        private IEnumerator HitRoutine(Pawn targetPawn, int hitCount, float hitDelay) {
            float damage = StatCalculator.GetDamage(OwnerPawn.force,
                OwnerPawn.BaseDamage * OwnerWeaponBundle.ActualStat.damageFactor, this);

            for (int i = 0; i < hitCount; ++i) {
                if (targetPawn == null) yield break;

                GameObject hitFX = OwnerWeaponBundle.weapon.prefabResources[2]
                    .InstantiateFX(targetPawn.transform.position, false);

                DamageInfo damageInfo = new(OwnerPawn, this, null, damage, 0f);
                targetPawn.Damage(damageInfo);

                yield return new WaitForSeconds(hitDelay);
            }
        }
    }
}