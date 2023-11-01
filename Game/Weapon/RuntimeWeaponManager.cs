using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class RuntimeWeaponManager : MonoBehaviour, IPauseable {
        public static RuntimeWeaponManager Instance { get; private set; }

        public const float SameWeaponDelay = 0.13f;

        public float BaseCooltime {
            get {
                float value = GameManager.Instance.PlayingGame.PlayingChapter.gameBalance.weaponCooltime;
                if (GamePlayer.LocalPlayer != null)
                    value *= GamePlayer.LocalPlayer.Pawn.ability.attackCooltimeScale;

                return value;
            }
        }

        private float elapsedTime;

        private readonly List<RuntimeWeaponBase> weaponList = new();


        private void Awake() {
            Instance = this;
        }

        private void Update() {
            if (!GameManager.Instance.IsPlaying) return;
            if (!GameManager.Instance.PlayingGame.IsBattlePhase) {
                elapsedTime = 0f;
                return;
            }

            if (GamePlayer.LocalPlayer == null) return;
            if (!GamePlayer.LocalPlayer.Pawn.isAlive) return;

            float deltaTime = Time.deltaTime;

            elapsedTime += deltaTime;

            SortDelayIndice();
            foreach (RuntimeWeaponBase weapon in weaponList) {
                int delayIndex = weapon.DelayIndex;
                WeaponStat stat = weapon.OwnerWeaponBundle.ActualStat;
                float actualCooltime = StatCalculator.GetWeaponCooltime(PawnForce.Player, BaseCooltime) *
                                       stat.cooltimeFactor;

                int session = (int)(elapsedTime / actualCooltime - SameWeaponDelay * delayIndex);

                if (weapon.Session != session) {
                    weapon.Session = session;

                    weapon.OnRepeatWeaponTick(actualCooltime);
                }
            }

            foreach (RuntimeWeaponBase weapon in weaponList) {
                weapon.OnTick(deltaTime);
            }
        }

        public void ClearWeapons() {
            RuntimeWeaponBase[] weapons = weaponList.ToArray();
            foreach (RuntimeWeaponBase weapon in weapons) {
                RemoveWeapon(weapon);
            }

            weaponList.Clear();
        }

        public void CreateInventoryWeapons() {
            foreach (WeaponBundle weapon in WeaponInventory.Instance.Slots) {
                if (weapon == null) continue;

                CreateWeapon(weapon);
            }

            SortDelayIndice();
        }

        public void CreateWeapon(WeaponBundle weaponBundle) {
            int count = StatCalculator.GetThrowWeaponCountFactor(PawnForce.Player, 1);

            for (int i = 0; i < count; ++i) {
                RuntimeWeaponBase runtimeWeapon =
                    RuntimeWeaponFactory.CreateRuntimeWeapon(weaponBundle, GamePlayer.LocalPlayer.Pawn);

                if (runtimeWeapon == null) continue;

                AddWeapon(runtimeWeapon);
                runtimeWeapon.OnEquip();
            }
        }

        private void AddWeapon(RuntimeWeaponBase runtimeWeapon) {
            weaponList.Add(runtimeWeapon);
        }

        private void RemoveWeapon(RuntimeWeaponBase runtimeWeapon) {
            runtimeWeapon.OnDestroy();

            weaponList.Remove(runtimeWeapon);
        }

        public void RemoveWeapon(WeaponBundle ownerWeapon) {
            RuntimeWeaponBase[] weapons = weaponList.ToArray();
            foreach (RuntimeWeaponBase weapon in weapons) {
                if (weapon.OwnerWeaponBundle == ownerWeapon)
                    RemoveWeapon(weapon);
            }
        }

        private void SortDelayIndice() {
            for (int weaponI = weaponList.Count - 1; weaponI >= 0; --weaponI) {
                RuntimeWeaponBase weapon = weaponList[weaponI];

                int sameWeaponCount = 0;
                for (int compareI = weaponI - 1; compareI >= 0; --compareI) {
                    RuntimeWeaponBase otherWeapon = weaponList[compareI];
                    if (weapon.OwnerWeaponBundle.weapon == otherWeapon.OwnerWeaponBundle.weapon)
                        ++sameWeaponCount;
                }

                weapon.DelayIndex = sameWeaponCount;
            }
        }
    }
}