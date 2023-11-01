using ArcaneSurvivorsClient.Game;

namespace ArcaneSurvivorsClient.Game {
    public interface IPawnBrain {
        public static Pawn PlayerPawn => GamePlayer.LocalPlayer?.Pawn;
        public Pawn Pawn { get; set; }

        public void OnSpawn();
        public void OnTick(float deltaTime);
        public void OnHit();
        public void OnDeath();
    }
}