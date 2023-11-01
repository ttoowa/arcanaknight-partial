using ArcaneSurvivorsClient.Locale;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class PotionGoods : IShopGoods {
        public bool IsSoldOut { get; private set; }

        public string DisplayName => name.ToLocale();
        public string DisplayDescription => description.ToLocale();
        public float Price { get; }

        public Sprite Icon => GameResource.Instance.potionSprite;

        public string name = "shop.goods.potion.name";
        public string description = "shop.goods.potion.desc";
        public event IShopGoods.SoldOutChangedDelegate SoldOutChanged;

        public PotionGoods(float price) {
            Price = price;
        }

        public void SetSoldOut(bool isSoldOut) {
            IsSoldOut = isSoldOut;

            SoldOutChanged?.Invoke(isSoldOut);
        }
    }
}