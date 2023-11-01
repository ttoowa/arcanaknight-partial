using System;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class WeaponBundle {
        public const int MinLevel = 1;
        public const int MaxLevel = 3;

        public Weapon weapon;

        //public IRuntimeWeapon runtimeWeapon;
        public int level = MinLevel;

        public float BoughtPrice { get; private set; }

        public float SellPrice => BoughtPrice;

        public Sprite ActualIcon => weapon.GetIcon(level);

        public WeaponStat ActualStat => weapon.GetStat(level);

        public float accDamage;

        public WeaponBundle(Weapon weapon) {
            this.weapon = weapon;
        }

        // public void SetRuntimeWeaponActive(bool active) {
        //     if (active) {
        //         if (runtimeWeapon != null)
        //             SetRuntimeWeaponActive(false);
        //
        //         runtimeWeapon = RuntimeWeaponFactory.CreateRuntimeWeapon(this, GamePlayer.LocalPlayer.Pawn);
        //         runtimeWeapon.OnEquip();
        //
        //         RuntimeWeaponManager.Instance.AddWeapon(runtimeWeapon);
        //     } else if (runtimeWeapon != null) {
        //         RuntimeWeaponManager.Instance.RemoveWeapon(runtimeWeapon);
        //
        //         runtimeWeapon.OnDestroy();
        //         runtimeWeapon = null;
        //     }
        // }

        public void SetBoughtPrice(float price) {
            BoughtPrice = price;
        }

        public void CollectAccDamage(float damage) {
            accDamage += damage;
        }

        // public void LevelUp() {
        //     level = Math.Clamp(level + 1, MinLevel, MaxLevel);
        // }
    }
}