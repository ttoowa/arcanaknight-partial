namespace ArcaneSurvivorsClient.Game {
    using UnityEngine;

    namespace ArcaneSurvivorsClient.Game {
        public class Weapon_PoisonCloud : RuntimeWeaponBase {
            public override void OnEquip() {
            }

            public override void OnDestroy() {
            }

            public override void OnRepeatWeaponTick(float cooltime) {
                switch (OwnerWeaponBundle.level) {
                    case 1:
                    case 2:
                        CreateCloud(OwnerWeaponBundle.weapon.prefabResources[0], 1f);
                        break;
                    case 3: {
                        CreateCloud(OwnerWeaponBundle.weapon.prefabResources[1], 0.3f);
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

            private void CreateCloud(GameObject prefab, float damageTick) {
                GameObject cloud = prefab.InstantiateFX(OwnerPawn.FXPosition, false, 3.5f);

                UnityEventListener eventListener = cloud.GetOrAdd<UnityEventListener>();
                eventListener.Events.OnTriggerEnter += (other) => {
                    if (other.attachedRigidbody == null) return;
                    Pawn pawn = other.attachedRigidbody.Get<Pawn>();
                    if (pawn == null || pawn.force != PawnForce.Monster) return;

                    Abnormal_Poison abnormal = new(pawn, OwnerPawn, this, 3f, OwnerPawn.GetActualDamage(this));
                    abnormal.damageTick = damageTick;
                    pawn.AbnormalSet.AddAbnormal(abnormal);
                };

                SfxPlayer.Play("weapon.poisonCloud.emit");
            }
        }
    }
}