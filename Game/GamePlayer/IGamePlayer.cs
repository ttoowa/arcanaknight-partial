using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public interface IGamePlayer {
        public Pawn Pawn { get; }

        public void OnTriggerEnter(Collider other);

        public void OnTriggerStay(Collider other);

        public void OnTriggerExit(Collider other);

        public void OnCollisionEnter(Collision other);

        public void OnCollisionStay(Collision other);

        public void OnCollisionExit(Collision other);
    }
}