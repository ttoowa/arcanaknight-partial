namespace ArcaneSurvivorsClient.Game {
    public abstract class RuntimeWeaponBase {
        public Pawn OwnerPawn { get; set; }

        public WeaponBundle OwnerWeaponBundle { get; set; }

        public int Session { get; set; }

        public int DelayIndex { get; set; }

        public abstract void OnEquip();

        public abstract void OnDestroy();

        public abstract void OnTick(float deltaTime);

        public abstract void OnRepeatWeaponTick(float cooltime);

        public abstract void OnHit(Pawn pawn);

        public abstract void OnLevelChanged(int level);
    }
}