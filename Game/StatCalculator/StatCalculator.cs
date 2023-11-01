using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public static class StatCalculator {
        private static GameDifficulty Difficulty =>
            GameManager.Instance.PlayingGame.PlayingChapter.difficultyState.SelectedDifficulty;

        /// <summary>
        ///     상점 상품 가격
        /// </summary>
        public static int GetShopPrice(float baseValue) {
            return (int)Difficulty.shopPriceEffect.statValue.Apply(baseValue);
        }

        /// <summary>
        ///     데미지
        /// </summary>
        public static float GetDamage(PawnForce force, float baseValue, RuntimeWeaponBase runtimeWeapon) {
            // runtimeWeapon을 인수로 받는 이유는 일부 특이케이스를 적용하기 위함
            if (force == PawnForce.Player) {
                float value = UpgradableStatManager.GetStatValue(UpgradableStatType.AttackPower).Apply(baseValue);
                return Difficulty.playerDamageEffect.statValue.Apply(value);
            } else if (force == PawnForce.Monster)
                return Difficulty.monsterDamageEffect.statValue.Apply(baseValue);

            return baseValue;
        }

        /// <summary>
        ///     공격 쿨타임
        /// </summary>
        public static float GetWeaponCooltime(PawnForce force, float baseValue) {
            if (force == PawnForce.Player) {
                float value = UpgradableStatManager.GetStatValue(UpgradableStatType.AttackCooltime).Apply(baseValue);
                float difficultyMultiplier = 1f / (Difficulty.playerAttackSpeedEffect.statValue.ActualValue + 1f);
                return value * difficultyMultiplier;
            }

            return baseValue;
        }

        /// <summary>
        ///     이동속도
        /// </summary>
        public static float GetMoveSpeed(PawnForce force, float baseValue) {
            if (force == PawnForce.Player) {
                float value = UpgradableStatManager.GetStatValue(UpgradableStatType.MoveSpeed).Apply(baseValue);
                return Difficulty.playerMoveSpeedEffect.statValue.Apply(value);
            } else if (force == PawnForce.Monster)
                return Difficulty.monsterMoveSpeedEffect.statValue.Apply(baseValue);

            return baseValue;
        }

        /// <summary>
        ///     최대체력
        /// </summary>
        public static float GetMaxHP(PawnForce force, float baseValue) {
            if (force == PawnForce.Player) {
                float value = UpgradableStatManager.GetStatValue(UpgradableStatType.HP).Apply(baseValue);
                return Difficulty.playerHpEffect.statValue.Apply(value);
            } else if (force == PawnForce.Monster)
                return Difficulty.monsterHpEffect.statValue.Apply(baseValue);

            return baseValue;
        }

        /// <summary>
        ///     체력 리젠
        /// </summary>
        public static float GetHpRegen(PawnForce force, float baseValue) {
            if (force == PawnForce.Player)
                return UpgradableStatManager.GetStatValue(UpgradableStatType.HpRegen).Apply(baseValue);
            return baseValue;
        }

        /// <summary>
        ///     처치 시 회복확률
        /// </summary>
        public static float GetHealProbability(PawnForce force, float baseValue) {
            return baseValue;
        }

        /// <summary>
        ///     넉백 배율
        /// </summary>
        public static float GetKnockbackFactor(PawnForce force, float baseValue) {
            return baseValue;
        }

        /// <summary>
        ///     공격 시 즉사확률
        /// </summary>
        public static float GetInstantDeathProbability(PawnForce force, float baseValue) {
            return baseValue;
        }

        /// <summary>
        ///     골드를 두배로 얻을 확률
        /// </summary>
        public static float GetDoubleGoldProbability(PawnForce force, float baseValue) {
            return baseValue;
        }

        /// <summary>
        ///     피격 방어확률
        /// </summary>
        public static float GetBlockProbability(PawnForce force, float baseValue) {
            return baseValue;
        }

        /// <summary>
        ///     공격 범위
        /// </summary>
        public static float GetAttackRange(PawnForce force, float baseValue, RuntimeWeaponBase runtimeWeapon) {
            return baseValue;
        }

        /// <summary>
        ///     투사체 개수
        /// </summary>
        public static int GetThrowWeaponCountFactor(PawnForce force, int baseValue) {
            return baseValue;
        }
    }
}