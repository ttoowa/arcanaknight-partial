using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class Weapon_SeekerMine : RuntimeWeaponBase {
        public override void OnEquip() {
        }

        public override void OnDestroy() {
        }

        public override void OnRepeatWeaponTick(float cooltime) {
            float speed = 6f;
            switch (OwnerWeaponBundle.level) {
                case 1: {
                    FireSeekerBall(speed, OwnerWeaponBundle.weapon.prefabResources[1]);
                    break;
                }
                case 2: {
                    int ballCount = 2;
                    for (int i = 0; i < ballCount; ++i) {
                        FireSeekerBall(speed, OwnerWeaponBundle.weapon.prefabResources[1]);
                    }

                    break;
                }
                case 3: {
                    int ballCount = 3;
                    for (int i = 0; i < ballCount; ++i) {
                        FireSeekerBall(speed, OwnerWeaponBundle.weapon.prefabResources[2]);
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

        private void FireSeekerBall(float speed, GameObject explosionPrefab) {
            GameObject ball = OwnerWeaponBundle.weapon.prefabResources[0]
                .InstantiateFX(OwnerPawn.FXPosition, false, 3f);

            MonsterPawn target = MonsterManager.Instance.GetRandomMonster();
            if (target == null) return;
            Vector3 normal = target.transform.position - ball.transform.position;
            normal.y = 0f;

            ThrowingObject throwingObject = ball.Get<ThrowingObject>();
            throwingObject.speed = speed;
            throwingObject.Normal = normal;

            UnityEventListener eventListener = ball.GetOrAdd<UnityEventListener>();
            eventListener.Events.OnTriggerEnter += (other) => {
                if (other.attachedRigidbody == null) return;
                Pawn pawn = other.attachedRigidbody.Get<Pawn>();
                if (pawn == null || pawn.force != PawnForce.Monster) return;

                GameObject explosion = explosionPrefab.InstantiateFX(ball.transform.position, false);
                explosion.InitPlayerHitboxes(OwnerPawn, this, null);

                ball.Destroy();
            };

            //SfxPlayer.Play("weapon.sword.swing", true);
        }
    }
}