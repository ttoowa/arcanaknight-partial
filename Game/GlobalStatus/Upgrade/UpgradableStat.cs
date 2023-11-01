using System;
using ArcaneSurvivorsClient.Locale;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [Serializable]
    [CreateAssetMenu(fileName = "UpgradableStat", menuName = "ScriptableObject/UpgradableStat", order = 1)]
    public class UpgradableStat : ScriptableObject, ILibraryData {
        public object Key => (int)type;


        public UpgradableStatType type;

        public Sprite iconSprite;

        [LocaleKey]
        public string name;

        [Tooltip("Level 단계 수")]
        public int maxLevel = 50;

        [Tooltip("MaxLevel 도달시 총 스탯값")]
        public StatValue totalValue;

        [Tooltip("최초 가격")]
        public int startPrice = 10;

        [Tooltip("MaxLevel 도달까지 필요한 총 가격")]
        public int desiredPriceSum = 1000;

        public float GetIncreasePrice(float desiredPriceSum) {
            if (startPrice * maxLevel > desiredPriceSum) {
                throw LogBuilder.BuildException(LogType.Error, nameof(UpgradableStat),
                    $"desiredPriceSum({desiredPriceSum}) is too small", new[] { new LogElement("name", name) });
            }

            return (-2f * startPrice + 2f * (float)this.desiredPriceSum / maxLevel) / maxLevel;
            // return SearchUtility.BinarySearch((float incrPrice) => {
            //     float priceSum = startPrice;
            //     for (int level = 0; level < maxLevel; ++level) {
            //         priceSum += startPrice + incrPrice * level;
            //     }
            //
            //     return priceSum;
            // }, desiredPriceSum, 0f, 1000f, 5f);
        }

        public int GetPrice(int level) {
            float incrPrice = GetIncreasePrice(desiredPriceSum);

            return (int)(startPrice + incrPrice * level);
        }
    }
}