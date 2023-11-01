using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public interface IShopGoods {
        public delegate void SoldOutChangedDelegate(bool isSoldOut);

        public bool IsSoldOut { get; }

        public string DisplayName { get; }
        public string DisplayDescription { get; }
        public float Price { get; }

        public Sprite Icon { get; }

        public event SoldOutChangedDelegate SoldOutChanged;

        public void SetSoldOut(bool isSoldOut);
    }

    public interface IShopGoodsUI {
        public IShopGoods Model { get; }
    }
}