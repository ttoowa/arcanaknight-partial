using System;

namespace ArcaneSurvivorsClient.Game {
    public class GameStatistics {
        public TimeSpan playtime;
        public int killCount;

        public float damageDealt;
        public float mostDamageDealt;
        public float damageTaken;
        public float hpHealed;
        public float collectedGold;
        public int purchasedWeapons;
        public int rerollCount;

        public void Reset() {
            playtime = TimeSpan.Zero;
            killCount = 0;
            damageDealt = 0;
            mostDamageDealt = 0;
            damageTaken = 0;
            hpHealed = 0;
            collectedGold = 0;
            purchasedWeapons = 0;
            rerollCount = 0;
        }

        public void CollectDamageDealt(PawnForce force, float damage) {
            if (force == PawnForce.Monster) {
                damageDealt += damage;

                if (mostDamageDealt < damage)
                    mostDamageDealt = damage;
            } else if (force == PawnForce.Player)
                damageTaken += damage;
        }

        public void CollectKill(int count = 1) {
            killCount += count;
        }

        public void CollectHealed(float hp) {
            hpHealed += hp;
        }

        public void CollectGoldCollected(float amount) {
            collectedGold += amount;
        }

        public void CollectWeaponPurchased() {
            purchasedWeapons++;
        }

        public void CollectReroll() {
            rerollCount++;
        }
    }
}