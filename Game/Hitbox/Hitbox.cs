using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient.Game {
    public delegate void HitDelegate(Pawn pawn);

    public class Hitbox : MonoBehaviour {
        public string ID => parentHitbox != null ? parentHitbox.ID : localId;

        private string localId;

        [Tooltip("파괴 시 제거할 오브젝트입니다. 비어있으면 히트박스만 파괴합니다.")]
        public GameObject destroyTarget;

        public Hitbox parentHitbox;
        public RuntimeWeaponBase ownerWeapon;
        public Pawn ownerPawn;

        [Tooltip("근접공격인지의 여부입니다.")]
        public bool isCloseAttack = true;

        public float damage = 1f;

        [Tooltip("지정된 횟수 타격 시 파괴됩니다. -1로 지정하면 무한한 수명을 가집니다.")]
        public int penetration = -1;

        [Tooltip("지정된 시간 이후 파괴됩니다. -1로 지정하면 사라지지 않습니다.")]
        public float lifetime = 0.1f;

        public float knockbackFactor = 1f;

        [Tooltip("데미지를 입히는 간격")]
        public float tickDuration = 10f;

        public event HitDelegate Hit;

        private float elapsedTime;

        public void GenerateNewId() {
            localId = Guid.NewGuid().ToString();
        }

        private void Start() {
            GenerateNewId();
        }

        private void FixedUpdate() {
            elapsedTime += Time.fixedDeltaTime;

            if (lifetime >= 0f && elapsedTime >= lifetime)
                DestroySelf();
        }

        private void OnTriggerStay(Collider other) {
            OnCollision(other);
        }

        private void OnCollisionStay(Collision collisionInfo) {
            OnCollision(collisionInfo.collider);
        }

        private void OnCollision(Collider other) {
            if (!other.CompareTag("Body"))
                return;

            Pawn pawn;
            if (other.attachedRigidbody != null)
                pawn = other.attachedRigidbody.GetComponent<Pawn>();
            else
                pawn = other.GetComponent<Pawn>();

            if (string.IsNullOrWhiteSpace(ID) || pawn == null || ownerPawn == null || pawn == ownerPawn ||
                pawn.force == ownerPawn.force)
                return;

            bool damaged = pawn.Damage(new DamageInfo(ownerPawn, ownerWeapon, ID, damage, tickDuration));

            if (damaged) {
                Hit?.Invoke(pawn);
                ownerWeapon?.OnHit(pawn);

                if (penetration >= 0) {
                    --penetration;
                    if (penetration <= 0)
                        DestroySelf();
                }

                if (pawn.force == PawnForce.Monster) {
                    float knockback = GameManager.Instance.PlayingGame.PlayingChapter.gameBalance.knockbackPower *
                                      knockbackFactor;
                    Vector3 center = isCloseAttack ? ownerPawn.WorldPosition : transform.position;
                    pawn.Knockback(
                        (pawn.transform.position - center).ToVector2(VectorUtility.Vector3ToVector2.XZtoXY).normalized,
                        knockback);
                }
            }
        }

        private void DestroySelf() {
            if (destroyTarget != null)
                Destroy(destroyTarget);
            else
                Destroy(gameObject);
        }
    }
}