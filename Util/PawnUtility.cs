using ArcaneSurvivorsClient.Game;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public static class PawnUtility {
        public static Pawn GetPawn(this Collider collider) {
            if (collider.attachedRigidbody == null) return null;

            return collider.attachedRigidbody.GetComponent<Pawn>();
        }
    }
}