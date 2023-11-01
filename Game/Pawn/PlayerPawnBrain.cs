using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class PlayerPawnBrain : IPawnBrain {
        public Pawn Pawn { get; set; }

        public void OnSpawn() {
        }

        public void OnTick(float deltaTime) {
            // Vector2 normalizedCursor = new(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
            // Pawn.SetAngle(Mathf.Atan2(normalizedCursor.x - 0.5f, normalizedCursor.y - 0.5f) * Mathf.Rad2Deg);
        }

        public void OnHit() {
        }

        public void OnDeath() {
        }
    }
}