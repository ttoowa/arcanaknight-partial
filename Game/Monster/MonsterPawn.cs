using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class MonsterPawn : Pawn {
        public int score;

        [HideInInspector]
        public Monster monsterModel;

        protected override void Awake() {
            base.Awake();

            force = PawnForce.Monster;
        }
    }
}