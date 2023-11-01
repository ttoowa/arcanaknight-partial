using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ArcaneSurvivorsClient.Game {
    public static class PawnFactory {
        public static Pawn Spawn(GameObject pawnPrefab, IPawnBrain brain) {
            Pawn pawn = Object.Instantiate(pawnPrefab).GetComponent<Pawn>();
            if (pawn == null) {
                throw new Exception(
                    $"[PawnFactory.Spawn] Pawn prefab must have Pawn component.\n  Prefab name : {pawnPrefab?.name}");
            }

            pawn.brain = brain;
            pawn.brain.Pawn = pawn;
            return pawn;
        }
    }
}