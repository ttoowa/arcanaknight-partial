using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class MonsterBrain_Drunker : IPawnBrain {
        private const float MaxAngleOffset = 60f;
        private const float AngleAccelDuration = 0.2f;
        private const float TurnSpeed = 10f;

        public Pawn Pawn { get; set; }

        private float angleOffset;
        private float angleOffsetAccel;

        private float timer;

        public void OnSpawn() {
        }

        public void OnTick(float deltaTime) {
            if (IPawnBrain.PlayerPawn == null)
                return;

            timer += deltaTime;
            if (timer >= AngleAccelDuration) {
                timer -= AngleAccelDuration;

                angleOffsetAccel = Random.Range(-1f, 1f);
            }

            angleOffset = Mathf.Clamp(angleOffset + angleOffsetAccel * TurnSpeed, -MaxAngleOffset, MaxAngleOffset);

            Vector3 toPlayerNormal = (IPawnBrain.PlayerPawn.transform.position - Pawn.transform.position).normalized;
            float toPlayerAngle = Mathf.Atan2(toPlayerNormal.x, toPlayerNormal.z) * Mathf.Rad2Deg;
            toPlayerAngle += angleOffset;
            toPlayerNormal = new Vector3(Mathf.Sin(toPlayerAngle * Mathf.Deg2Rad), 0f,
                Mathf.Cos(toPlayerAngle * Mathf.Deg2Rad));

            Pawn.SetAngleToPosition(IPawnBrain.PlayerPawn.transform.position);
            Pawn.Move(new Vector2(toPlayerNormal.x, toPlayerNormal.z));
        }

        public void OnHit() {
        }

        public void OnDeath() {
        }
    }
}