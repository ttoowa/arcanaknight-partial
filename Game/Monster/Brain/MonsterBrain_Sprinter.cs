using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class MonsterBrain_Sprinter : IPawnBrain {
        public Pawn Pawn { get; set; }

        private bool IsSprinting => sprintTimer > sprintDelay;
        private float TotalDuration => sprintDuration + sprintDelay;

        private float sprintTimer;
        private float sprintDuration = 1f;
        private float sprintDelay = 3.5f;
        private float sprintSpeed = 2f;
        private float walkSpeed = 0.3f;

        public void OnSpawn() {
        }

        public void OnTick(float deltaTime) {
            if (IPawnBrain.PlayerPawn == null)
                return;

            // Update timer
            sprintTimer += deltaTime;
            if (sprintTimer >= TotalDuration)
                sprintTimer -= TotalDuration;

            Vector3 toPlayerNormal = (IPawnBrain.PlayerPawn.transform.position - Pawn.transform.position).normalized;

            Pawn.SetAngleToPosition(IPawnBrain.PlayerPawn.transform.position);
            Pawn.moveSpeedFactor = IsSprinting ? sprintSpeed : walkSpeed;
            Pawn.Move(new Vector2(toPlayerNormal.x, toPlayerNormal.z));
        }

        public void OnHit() {
        }

        public void OnDeath() {
        }
    }
}