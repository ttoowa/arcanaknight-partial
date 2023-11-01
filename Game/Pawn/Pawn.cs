using System;
using ArcaneSurvivorsClient.Game;
using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient.Game {
    public class Pawn : MonoBehaviour, IPauseable {
        public delegate void PawnDelegate(Pawn pawn);

        public delegate void DamagedDelegate(Pawn pawn, DamageInfo damageInfo);

        public delegate void HealedDelegate(Pawn pawn, float amount);

        public const float RemoveCorpseDelay = 1f;

        // Inspector fields
        public IGamePlayer player;
        public IPawnBrain brain;
        public PawnForce force;
        public GameObject graphic;
        public GameObject colliderArea;
        public GameObject deathFX;
        public Animator characterAnimator;
        public SpriteRenderer characterSpriteRenderer;
        public Transform headLocator;

        // Status
        public bool IsFullHp => hp.Value >= ActualMaxHp;
        public float NormalizedHP => hp.Value / ActualMaxHp;

        public bool IsAlive => isAlive;

        public float BaseDamage =>
            GameManager.Instance.PlayingGame.PlayingChapter.gameBalance.GetDamage(force) * ability.damageScale;

        public float BaseMoveSpeed => GameManager.Instance.PlayingGame.PlayingChapter.gameBalance.GetMoveSpeed(force) *
                                      ability.moveSpeedScale * moveSpeedFactor;

        public float ActualMoveSpeed => StatCalculator.GetMoveSpeed(force, BaseMoveSpeed);

        public float ActualMaxHp => StatCalculator.GetMaxHP(force, ability.hp);
        public float ActualHpRegen => StatCalculator.GetHpRegen(force, ability.hpRegen);

        [HideInInspector]
        public bool isAlive;

        [HideInInspector]
        public PawnAbility ability;

        public RuntimeValue<float> hp = new();

        private float hpRegenTimer;

        // Combat
        private readonly DamageHistory damageHistory = new();
        public AbnormalSet AbnormalSet { get; private set; }

        // Physics
        [HideInInspector]
        public float moveSpeedFactor = 1f;

        private new Rigidbody rigidbody;
        private Vector2 velocity;
        private Vector2 knockbackVelocity;

        // Graphic
        public Vector2 CanvasPosition => UIUtility.WorldToCanvasPoint(transform.position);

        public Vector3 WorldPosition {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }

        public Vector3 BodyPosition => (FootPosition + HeadPosition) * 0.5f;

        public Vector3 FootPosition => WorldPosition;
        public Vector3 FXPosition => (HeadPosition + WorldPosition) * 0.5f;

        public Vector3 HeadPosition => headLocator.position;

        public float CurrentAngle => currentAngle;

        public Vector3 TargetAngleNormalVec3 {
            get {
                float radAngle = Mathf.Deg2Rad * targetAngle;
                return new Vector3(Mathf.Sin(radAngle), 0f, Mathf.Cos(radAngle));
            }
        }

        public Vector2 TargetAngleNormalVec2 {
            get {
                float radAngle = Mathf.Deg2Rad * targetAngle;
                return new Vector2(Mathf.Sin(radAngle), Mathf.Cos(radAngle));
            }
        }

        private float currentAngle;
        private float targetAngle;

        private float removeCorpseTimer;

        private bool animationUpdated;

        private const float DamagedAnimationDuration = 0.3f;
        private float damagedAnimationTimer;

        private bool isKinematicState;

        public event PawnDelegate Killed;
        public event PawnDelegate Removed;
        public event DamagedDelegate Damaged;

        public event HealedDelegate Healed;

        protected virtual void Awake() {
            rigidbody = GetComponent<Rigidbody>();
            AbnormalSet = new AbnormalSet(this);
            if (!LifecycleState.IsInitialized) {
                Debug.Log("LifecycleState is not initialized.");
                return;
            }

            ResetState();

            Vector3 position = transform.localPosition;
            position.y = 0f;
            transform.localPosition = position;
        }

        protected virtual void Start() {
            brain?.OnSpawn();

            PauseManager.Instance.PauseStateChanged += OnPauseStateChanged;
        }

        protected void OnDestroy() {
            PauseManager.Instance.PauseStateChanged -= OnPauseStateChanged;
        }

        protected virtual void Update() {
            if (!GameManager.Instance.IsPlaying) return;
            float deltaTime = Time.deltaTime;

            if (isAlive) {
                damageHistory.OnTick(deltaTime);
                brain?.OnTick(deltaTime);
                AbnormalSet?.OnTick(deltaTime);

                if (!animationUpdated)
                    SetAnimationState(AnimationState.Idle);
            }

            UpdateRotation();
            UpdateDamagedAnimation();
            UpdateHpRegen(deltaTime);

            if (!isAlive && force == PawnForce.Monster) {
                removeCorpseTimer += deltaTime;
                if (removeCorpseTimer >= RemoveCorpseDelay)
                    Destroy(gameObject);
            }
        }

        private void FixedUpdate() {
            if (!GameManager.Instance.IsPlaying) return;

            UpdatePosition();
            UpdatePhysics();
        }

        private void LateUpdate() {
            if (!GameManager.Instance.IsPlaying) return;

            animationUpdated = false;
        }

        public void ResetState() {
            isAlive = true;
            removeCorpseTimer = 0f;
            if (deathFX != null)
                deathFX.SetActive(false);

            if (colliderArea != null)
                colliderArea.SetActive(true);
            damageHistory.Clear();
            HealFull(false);
        }

        // Pawn functions
        public void Kill() {
            if (!isAlive) return;

            isAlive = false;
            brain?.OnDeath();
            SetAnimationState(AnimationState.Death);

            Killed?.Invoke(this);

            if (deathFX != null)
                deathFX.SetActive(true);

            if (colliderArea != null)
                colliderArea.SetActive(false);
        }

        public void Remove() {
            Destroy(gameObject);

            Removed?.Invoke(this);
        }

        public bool DamageSimple(Pawn sourcePawn, RuntimeWeaponBase sourceWeapon, float damage) {
            return Damage(new DamageInfo(sourcePawn, sourceWeapon, null, damage, 0f));
        }

        public bool Damage(DamageInfo damageInfo) {
            if (!GameManager.Instance.PlayingGame.IsBattlePhase) return false;
            if (PauseManager.Instance.IsPaused) return false;
            if (damageInfo == null || damageInfo.damage < 0) return false;
            if (!damageInfo.sourcePawn.IsAlive) return false;
            if (!damageHistory.Add(damageInfo)) return false;

            // Damaged
            hp.Value -= damageInfo.Damage;

            damagedAnimationTimer = DamagedAnimationDuration;

            // Collect statistics
            if (damageInfo.sourceWeapon != null)
                damageInfo.sourceWeapon.OwnerWeaponBundle.CollectAccDamage(damageInfo.damage);

            // Event
            Damaged?.Invoke(this, damageInfo);

            if (hp.Value <= 0f) {
                hp.Value = 0f;
                Kill();
            }

            return true;
        }

        public void Heal(float heal) {
            if (heal < 0)
                return;

            SetHp(hp.Value + heal);
        }

        public void HealPercent(float percent) {
            Heal(ActualMaxHp * percent);
        }

        public void HealFull(bool withEvent = true) {
            SetHp(ActualMaxHp, withEvent);
        }

        public void SetHp(float hp, bool withEvent = true) {
            float delta = hp - this.hp.Value;

            this.hp.Value = Mathf.Clamp(hp, 0f, ActualMaxHp);

            if (withEvent) {
                if (delta > 0)
                    Healed?.Invoke(this, delta);
            }
        }

        public void Move(Vector2 normal) {
            if (!isAlive) return;

            velocity += normal.normalized * GetActualMoveForce() * Time.deltaTime;

            SetAngle(Mathf.Atan2(normal.x, normal.y) * Mathf.Rad2Deg);

            SetAnimationState(AnimationState.Run);

            if (characterSpriteRenderer != null) {
                if (normal.x > 0)
                    graphic.transform.localScale = new Vector3(1f, 1f, 1f);
                else if (normal.x < 0)
                    graphic.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }

        public void Knockback(Vector2 normal, float power) {
            float defaultKnockbackPower = 0.15f;

            power *= defaultKnockbackPower;
            float knockbackForce = StatCalculator.GetKnockbackFactor(PawnForce.Player, power);

            knockbackVelocity = normal.normalized * knockbackForce;
        }

        public void Attack() {
        }

        public void SetAngle(float angle) {
            targetAngle = angle;
        }

        public void SetAngleToPosition(Vector3 position) {
            Vector3 targetDir = position - transform.position;
            float angle = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;
            targetAngle = angle;
        }

        public void SetAnimationState(AnimationState state) {
            if (characterAnimator == null) return;

            characterAnimator.SetInteger("State", (int)state);
            animationUpdated = true;
        }

        // Updates
        private void UpdatePosition() {
            float velocityDamper = 1f - Mathf.Clamp01(ability.friction * Time.deltaTime * 60f);
            float knockbackDamper = 1f - Mathf.Clamp01(0.05f * Time.deltaTime * 60f);

            velocity *= velocityDamper;
            knockbackVelocity *= knockbackDamper;

            rigidbody.MovePosition(rigidbody.position +
                                   new Vector3(velocity.x + knockbackVelocity.x, 0f, velocity.y + knockbackVelocity.y));
        }

        private void UpdateRotation() {
            if (targetAngle - currentAngle > 180f)
                currentAngle += 360f;
            else if (targetAngle - currentAngle < -180f)
                currentAngle -= 360f;

            currentAngle += (targetAngle - currentAngle) * 0.2f;

            Vector2 normal = TargetAngleNormalVec2;
            characterAnimator.SetFloat("MoveX", normal.x);
            characterAnimator.SetFloat("MoveY", normal.y);
        }

        private void UpdatePhysics() {
            rigidbody.velocity *= 1f - Mathf.Clamp01(ability.friction * Time.deltaTime * 60f);
            //rigidbody.velocity *= 1f - Mathf.Clamp01(ability.friction * Mathf.Pow(Time.deltaTime, 1.06f) * 60f);
        }

        private void UpdateDamagedAnimation() {
            damagedAnimationTimer = Mathf.Max(0f, damagedAnimationTimer - Time.deltaTime);
            if (damagedAnimationTimer > 0f)
                characterSpriteRenderer.material.SetColor("_AddColor", new Color(1f, 1f, 1f, 1f));
            else
                characterSpriteRenderer.material.SetColor("_AddColor", new Color(1f, 1f, 1f, 0f));
        }

        private void UpdateHpRegen(float deltaTime) {
            if (GameManager.Instance.PlayingGame.IsBattlePhase && isAlive) {
                hpRegenTimer += deltaTime;
                if (hpRegenTimer >= 1f) {
                    hpRegenTimer -= 1f;

                    Heal(ActualHpRegen);
                }
            }
        }
        // Unity event

        private void OnTriggerEnter(Collider other) {
            player?.OnTriggerEnter(other);
        }

        private void OnTriggerStay(Collider other) {
            player?.OnTriggerStay(other);
        }

        private void OnTriggerExit(Collider other) {
            player?.OnTriggerExit(other);
        }

        private void OnCollisionEnter(Collision collision) {
            player?.OnCollisionEnter(collision);
        }

        private void OnCollisionStay(Collision collision) {
            player?.OnCollisionStay(collision);
        }

        private void OnCollisionExit(Collision collision) {
            player?.OnCollisionExit(collision);
        }

        private void OnPauseStateChanged(bool paused) {
            if (paused) {
                isKinematicState = rigidbody.isKinematic;
                rigidbody.isKinematic = paused;
            } else
                rigidbody.isKinematic = isKinematicState;
        }

        // Stat
        public float GetActualDamage(RuntimeWeaponBase weapon = null) {
            float damage = BaseDamage;
            if (weapon != null)
                damage *= weapon.OwnerWeaponBundle.ActualStat.damageFactor;
            return StatCalculator.GetDamage(force, damage, weapon);
        }

        private float GetActualMoveForce() {
            return PhysicsUtility.CalcMoveForce(ActualMoveSpeed, ability.friction);
        }

        // Graphics
        public void SetMaterialBool(string fieldName, bool value) {
            if (characterSpriteRenderer == null) return;

            characterSpriteRenderer.material.SetInt(fieldName, value ? 1 : 0);
        }
    }
}