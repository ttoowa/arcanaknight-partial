using System;

namespace ArcaneSurvivorsClient.Game {
    public class Abnormal {
        public string guid;

        public Pawn sourcePawn;
        public RuntimeWeaponBase sourceWeapon;
        public Pawn pawn;
        public AbnormalType type;
        public AbnormalEffect effect;
        public float leftTime;


        public Abnormal(Pawn pawn, Pawn sourcePawn, RuntimeWeaponBase sourceWeapon, float leftTime) {
            guid = Guid.NewGuid().ToString();
            this.pawn = pawn;
            this.sourcePawn = sourcePawn;
            this.sourceWeapon = sourceWeapon;
            this.leftTime = leftTime;
        }

        public virtual void OnEnabled() {
        }

        public virtual void OnTick(float deltaTime) {
            leftTime -= deltaTime;
        }

        public virtual void OnDisabled() {
        }
    }
}