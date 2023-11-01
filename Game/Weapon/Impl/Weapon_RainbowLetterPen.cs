using System.Collections.Generic;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class Weapon_RainbowLetterPen : RuntimeWeaponBase {
        private GameObject projectile;
        private Pawn targetPawn;

        private Vector2 velocity;

        // Star 1, 2 비행방식 가속력
        private float accel_Star1 = 4f;
        private float accel_Star2 = 8f;
        private float friction_Star1 = 0.08f;
        private float friction_Star2 = 0.03f;

        // Star 3 속도
        private float speed = 85f;

        private float range = 11.5f;

        private SimpleTimer emitTimer = new(1f);
        private SimpleTimer patternTimer = new(1f);

        private bool followRandom;

        public override void OnEquip() {
        }

        public override void OnDestroy() {
            DestroyProjectile();
        }

        public override void OnTick(float deltaTime) {
            if (!GameManager.Instance.IsPlaying || !GameManager.Instance.PlayingGame.IsBattlePhase) return;

            if (projectile == null) {
                if (emitTimer.Tick(deltaTime)) {
                    emitTimer.Reset();

                    EmitProjectile();
                } else
                    return;
            }

            switch (OwnerWeaponBundle.level) {
                case 1:
                case 2:
                    if (patternTimer.Tick(deltaTime))
                        UpdateTargetPawn();
                    break;
                case 3:
                    if (targetPawn == null || !targetPawn.isAlive ||
                        Vector3.Distance(targetPawn.WorldPosition, OwnerPawn.WorldPosition) < 0.2f)
                        UpdateTargetPawn();
                    break;
            }

            UpdateFollowTargetPawn(deltaTime);
        }

        public override void OnRepeatWeaponTick(float cooltime) {
        }

        public override void OnHit(Pawn pawn) {
            SfxPlayer.Play("weapon.rainbowLetterPen.hit", true);
        }

        public override void OnLevelChanged(int level) {
        }

        private void EmitProjectile() {
            if (projectile != null)
                DestroyProjectile();

            projectile = OwnerWeaponBundle.weapon.prefabResources[0]
                .Instantiate(FXResource.Instance.FXArea, OwnerPawn.WorldPosition);

            projectile.InitPlayerHitboxes(OwnerPawn, this, (hitbox) => {
                hitbox.tickDuration = OwnerWeaponBundle.ActualStat.cooltimeFactor;
            });
        }

        private void DestroyProjectile() {
            if (projectile == null) return;

            Object.Destroy(projectile);
            projectile = null;
        }

        private void UpdateTargetPawn() {
            targetPawn = MonsterManager.Instance.GetNearRandomMonster(range);
        }

        private void UpdateFollowTargetPawn(float deltaTime) {
            if (targetPawn == null) return;

            Vector2 diff =
                (targetPawn.BodyPosition - projectile.transform.position).ToVector2(VectorUtility.Vector3ToVector2
                    .XZtoXY);
            Vector2 direction = diff.normalized;
            float distance = diff.magnitude;

            switch (OwnerWeaponBundle.level) {
                case 1:
                case 2: {
                    float accel;
                    float firction;
                    if (OwnerWeaponBundle.level == 1) {
                        accel = accel_Star1;
                        firction = friction_Star1;
                    } else {
                        accel = accel_Star2;
                        firction = friction_Star2;
                    }

                    velocity.x += Mathf.Sign(direction.x) * accel * deltaTime;
                    velocity.y += Mathf.Sign(direction.y) * accel * deltaTime;

                    // TODO : 낮은 FPS에서 마찰력이 너무 높게 적용되는 문제 해결 필요
                    velocity *= 1f - Mathf.Clamp01(firction * deltaTime * 60f);

                    projectile.transform.localPosition += new Vector3(velocity.x, 0f, velocity.y);

                    Debug.DrawLine(projectile.transform.position, targetPawn.BodyPosition, Color.red);
                    break;
                }
                case 3: {
                    projectile.transform.position += new Vector3(direction.x, 0f, direction.y) *
                                                     Mathf.Min(distance, speed * deltaTime);
                    break;
                }
            }
        }
    }
}