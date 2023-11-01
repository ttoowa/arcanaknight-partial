using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class MonsterBrain_Bow : IPawnBrain {
        private const float MoveDelay = 3f;
        private const float ChargeDuration = 0.5f;
        private const float FireDuration = 1f;
        private const float TotalDuration = MoveDelay + ChargeDuration + FireDuration;

        public Pawn Pawn { get; set; }

        private float timer;

        private bool isCharged;
        private bool isFired;

        private float velocity;

        public void OnSpawn() {
        }

        public void OnTick(float deltaTime) {
            if (IPawnBrain.PlayerPawn == null)
                return;

            timer += deltaTime;
            if (timer >= TotalDuration) {
                timer -= TotalDuration;

                isCharged = false;
                isFired = false;
            }

            bool isCharging = false;
            if (timer > MoveDelay) {
                isCharging = timer < MoveDelay + ChargeDuration;

                if (isCharging) {
                    if (!isCharged) {
                        isCharged = true;

                        velocity = 2f;
                    }
                } else {
                    if (!isFired) {
                        isFired = true;

                        velocity = 6f;
                    }
                }
            }

            velocity *= 0.96f;

            Pawn.moveSpeedFactor = velocity;
            Vector3 toPlayerNormal = (IPawnBrain.PlayerPawn.transform.position - Pawn.transform.position).normalized;
            Pawn.SetAngleToPosition(IPawnBrain.PlayerPawn.transform.position);

            if (isCharging)
                Pawn.Move(new Vector2(-toPlayerNormal.x, -toPlayerNormal.z));
            else
                Pawn.Move(new Vector2(toPlayerNormal.x, toPlayerNormal.z));
        }

        public void OnHit() {
        }

        public void OnDeath() {
        }
    }
}