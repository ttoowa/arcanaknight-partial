using System;
using ArcaneSurvivorsClient.Locale;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class WeaponGoods : IShopGoods {
        public bool IsSoldOut { get; private set; }

        public string DisplayName => Weapon.name.ToLocale();
        public string DisplayDescription => Weapon.description.ToLocale();
        public float Price { get; }

        public Sprite Icon => Weapon.GetIcon(0);

        public WeaponType type;

        public event IShopGoods.SoldOutChangedDelegate SoldOutChanged;

        public Weapon Weapon => WeaponResource.Instance.library.GetData(type);

        public WeaponGoods(WeaponType type, float price) {
            this.type = type;
            Price = price;
        }

        public void SetSoldOut(bool isSoldOut) {
            IsSoldOut = isSoldOut;

            SoldOutChanged?.Invoke(isSoldOut);
        }
    }
}