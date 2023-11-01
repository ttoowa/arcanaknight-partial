namespace ArcaneSurvivorsClient.Game {
    public class Abnormal_Poison : Abnormal {
        public float damagePerSec;
        public float damageTick = 0.5f;
        private float damageTimer;


        public Abnormal_Poison(Pawn pawn, Pawn sourcePawn, RuntimeWeaponBase sourceWeapon, float leftTime,
            float damagePerSec) : base(pawn, sourcePawn, sourceWeapon, leftTime) {
            type = AbnormalType.Poison;
            effect = AbnormalEffect.Harmful;
            this.damagePerSec = damagePerSec;
        }

        public override void OnEnabled() {
            base.OnEnabled();
        }

        public override void OnTick(float deltaTime) {
            base.OnTick(deltaTime);

            damageTimer += deltaTime;
            if (damageTimer >= damageTick) {
                damageTimer -= damageTick;

                pawn.DamageSimple(sourcePawn, sourceWeapon, damagePerSec * damageTick);
            }
        }

        public override void OnDisabled() {
            base.OnDisabled();
        }
    }
}