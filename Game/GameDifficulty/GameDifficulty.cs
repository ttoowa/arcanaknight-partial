using System;
using ArcaneSurvivorsClient.Locale;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    [CreateAssetMenu(fileName = "GameDifficulty", menuName = "ScriptableObject/GameDifficulty", order = 1)]
    public class GameDifficulty : ScriptableObject, ILibraryData {
        public object Key => level;

        [LocaleKey]
        public string desc;

        public GameDifficultyEffect[] Effects { get; private set; }

        public int level;

        public GameDifficultyEffect shopPriceEffect;
        public GameDifficultyEffect monsterHpEffect;
        public GameDifficultyEffect monsterDamageEffect;
        public GameDifficultyEffect monsterMoveSpeedEffect;
        public GameDifficultyEffect playerHpEffect;
        public GameDifficultyEffect playerDamageEffect;
        public GameDifficultyEffect playerAttackSpeedEffect;
        public GameDifficultyEffect playerMoveSpeedEffect;

        public void Init() {
            shopPriceEffect.header = "difficulty.effect.shopPrice";
            monsterHpEffect.header = "difficulty.effect.monsterHp";
            monsterDamageEffect.header = "difficulty.effect.monsterDamage";
            monsterMoveSpeedEffect.header = "difficulty.effect.monsterMoveSpeed";
            playerHpEffect.header = "difficulty.effect.playerHp";
            playerDamageEffect.header = "difficulty.effect.playerDamage";
            playerAttackSpeedEffect.header = "difficulty.effect.playerAttackSpeed";
            playerMoveSpeedEffect.header = "difficulty.effect.playerMoveSpeed";

            Effects = new[] {
                shopPriceEffect,
                monsterHpEffect,
                monsterDamageEffect,
                monsterMoveSpeedEffect,
                playerHpEffect,
                playerDamageEffect,
                playerAttackSpeedEffect,
                playerMoveSpeedEffect
            };
        }
    }
}