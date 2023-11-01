using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class MonsterBrain_Walker : IPawnBrain {
        public Pawn Pawn { get; set; }

        public void OnSpawn() {
        }

        public void OnTick(float deltaTime) {
            if (IPawnBrain.PlayerPawn == null)
                return;

            Vector3 toPlayerNormal = (IPawnBrain.PlayerPawn.transform.position - Pawn.transform.position).normalized;

            Pawn.SetAngleToPosition(IPawnBrain.PlayerPawn.transform.position);
            Pawn.Move(new Vector2(toPlayerNormal.x, toPlayerNormal.z));
        }

        public void OnHit() {
        }

        public void OnDeath() {
        }
    }
}