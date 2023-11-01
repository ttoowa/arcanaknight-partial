using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient.Game {
    [CreateAssetMenu(fileName = "GameBalance", menuName = "ScriptableObject/GameBalance", order = 1)]
    public class GameBalance : ScriptableObject {
#if UNITY_EDITOR
        [Header("그래프 프리뷰 열 수")]
        public int previewSampleCount = 15;
#endif

        [Header("------ Stat ------")]
        public float weaponCooltime = 2.5f;

        public float defaultPlayerDamage = 15f;

        public float defaultPlayerMoveSpeed = 8f;

        public float defaultMonsterDamage = 1f;
        public float defaultMonsterMoveSpeed = 4f;

        [Header("------ Env ------")]
        public BalanceRange potionFieldDropPercent = new(0.05f, 0.025f);

        public float potionFieldHealAmountPercent = 0.2f;

        public float knockbackPower = 1f;


        [Header("------ Stage ------")]
        public BalanceRange stageDurationRange = new(45, 450);


        [Range(0, 1)]
        public float targetScorePercent;

        [Header("------ Monster Spawn ------")]
        public BalanceRange monsterSpawnHpSumRange;

        public BalanceRange monsterSpawnStepRange = new(6, 12);
        public BalanceRange monsterSpawnSetMaxElementCount = new(2, 4);
        public BalanceRange monsterSpawnAccel = new(0.05f, 0.6f);

        [Header("------ Item Price ------")]
        public BalanceRange weaponPriceRange;

        public BalanceRange potionPriceRange;
        public BalanceRange rerollPriceRange;

        public float GetDamage(PawnForce force) {
            return force switch {
                PawnForce.Player => defaultPlayerDamage,
                PawnForce.Monster => defaultMonsterDamage,
                _ => 0f
            };
        }

        public float GetMoveSpeed(PawnForce force) {
            return force switch {
                PawnForce.Player => defaultPlayerMoveSpeed,
                PawnForce.Monster => defaultMonsterMoveSpeed,
                _ => 0f
            };
        }
    }
}