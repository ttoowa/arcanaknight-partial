using System;
using System.Collections.Generic;
using ArcaneSurvivorsClient.Game.ArcaneSurvivorsClient.Game;

namespace ArcaneSurvivorsClient.Game {
    public static class RuntimeWeaponFactory {
        private static readonly Dictionary<WeaponType, Type> runtimeWeaponDict = new() {
            { WeaponType.LongSword, typeof(Weapon_Sword) },
            { WeaponType.FireScroll, typeof(Weapon_FireScroll) },
            { WeaponType.Spear, typeof(Weapon_Spear) },
            { WeaponType.SeekerMine, typeof(Weapon_SeekerMine) },
            { WeaponType.PoisonCloud, typeof(Weapon_PoisonCloud) },
            { WeaponType.GoldenFork, typeof(Weapon_GoldenFork) },
            { WeaponType.RainbowLetterPen, typeof(Weapon_RainbowLetterPen) },
            { WeaponType.Dagger, typeof(Weapon_Dagger) },
            { WeaponType.Static, typeof(Weapon_Static) }
        };

        public static RuntimeWeaponBase CreateRuntimeWeapon(WeaponBundle weaponBundle, Pawn ownerPawn) {
            runtimeWeaponDict.TryGetValue(weaponBundle.weapon.weaponType, out Type runtimeWeaponType);

            if (runtimeWeaponType == null) return null;

            RuntimeWeaponBase runtimeWeapon = Activator.CreateInstance(runtimeWeaponType) as RuntimeWeaponBase;

            runtimeWeapon.OwnerWeaponBundle = weaponBundle;
            runtimeWeapon.OwnerPawn = ownerPawn;
            return runtimeWeapon;
        }
    }
}