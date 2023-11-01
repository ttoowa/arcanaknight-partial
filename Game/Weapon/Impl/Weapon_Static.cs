using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class Weapon_Static : RuntimeWeaponBase {
        private const float DelayTime = 0.06f;

        public override void OnEquip() {
        }

        public override void OnDestroy() {
        }

        public override void OnRepeatWeaponTick(float cooltime) {
            // [0] : MainDetector
            // [1] : SubDetector
            // [2] : LightningFX
            // [3] : HitFX

            //if (MonsterManager.Instance.NearestMonster == null) return;
            int hitCount;
            switch (OwnerWeaponBundle.level) {
                default:
                case 1:
                    hitCount = 2;
                    break;
                case 2:
                    hitCount = 6;
                    break;
                case 3:
                    hitCount = 20;
                    break;
            }

            CoroutineDispatcher.Dispatch(AttackRoutine(hitCount));
        }

        public override void OnTick(float deltaTime) {
        }

        public override void OnHit(Pawn pawn) {
        }

        public override void OnLevelChanged(int level) {
        }

        private IEnumerator AttackRoutine(int maxHitCount) {
            HashSet<Pawn> targetPawnSet = new();
            HashSet<Pawn> hitPawnSet = new();

            GameObject mainDetector = OwnerWeaponBundle.weapon.prefabResources[0].InstantiateFX(OwnerPawn.FootPosition);
            mainDetector.GetOrAdd<UnityEventListener>().Events.OnTriggerStay += (Collider collider) => {
                Pawn pawn = collider.GetPawn();
                if (pawn != null && pawn.force != OwnerPawn.force)
                    targetPawnSet.Add(pawn);
            };


            yield return new WaitForSeconds(DelayTime);
            yield return new WaitForFixedUpdate();

            mainDetector.Destroy();
            if (targetPawnSet.Count == 0)
                yield break;

            Pawn targetPawn = targetPawnSet.GetRandom();
            targetPawnSet.Clear();
            if (!HitPawn(targetPawn, OwnerPawn)) yield break;
            hitPawnSet.Add(targetPawn);

            for (int i = 0; i < maxHitCount; ++i) {
                GameObject subDetector = OwnerWeaponBundle.weapon.prefabResources[1]
                    .InstantiateFX(targetPawn.FootPosition);
                subDetector.GetOrAdd<UnityEventListener>().Events.OnTriggerStay += (Collider collider) => {
                    Pawn pawn = collider.GetPawn();
                    if (pawn != null && pawn.force != OwnerPawn.force && !hitPawnSet.Contains(pawn))
                        targetPawnSet.Add(pawn);
                };

                yield return new WaitForSeconds(DelayTime);
                yield return new WaitForFixedUpdate();

                subDetector.Destroy();
                if (targetPawnSet.Count == 0)
                    yield break;

                Pawn prevTargetPawn = targetPawn;
                targetPawn = targetPawnSet.GetRandom();
                targetPawnSet.Clear();
                if (!HitPawn(targetPawn, prevTargetPawn)) yield break;
                hitPawnSet.Add(targetPawn);
            }
        }

        private bool HitPawn(Pawn targetPawn, Pawn prevTargetPawn) {
            if (targetPawn == null) return false;

            Vector3 prevPosition = prevTargetPawn != null ? prevTargetPawn.FXPosition : targetPawn.FXPosition;

            LineRenderer lightningFX = OwnerWeaponBundle.weapon.prefabResources[2].InstantiateFX(null)
                .GetComponentInChildren<LineRenderer>();
            lightningFX.SetPositions(new Vector3[] { prevPosition, targetPawn.FXPosition });

            GameObject hitFX = OwnerWeaponBundle.weapon.prefabResources[3].InstantiateFX(targetPawn.FXPosition);

            float damage = StatCalculator.GetDamage(OwnerPawn.force,
                OwnerPawn.BaseDamage * OwnerWeaponBundle.ActualStat.damageFactor, this);
            targetPawn.DamageSimple(OwnerPawn, this, damage);

            SfxPlayer.Play("weapon.static.hit", true);

            return true;
        }
    }
}