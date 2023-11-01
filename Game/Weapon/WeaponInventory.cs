using System;
using System.Collections.Generic;
using System.Linq;
using ArcaneSurvivorsClient.Analytics;
using Firebase.Analytics;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class WeaponInventory : MonoBehaviour, IPauseable {
        public struct AddWeaponResult {
            public bool result;
            public WeaponBundle bundle;

            public AddWeaponResult(bool result, WeaponBundle bundle) {
                this.result = result;
                this.bundle = bundle;
            }
        }

        public static WeaponInventory Instance { get; private set; }

        public delegate void WeaponSlotUpdatedDelegate();

        public delegate void WeaponSlotMergedDelegate(int targetSlotIndex, WeaponBundle slot);

        public const int MergeSourceCount = 3;
        public const int SlotCount = 5;

        public int WeaponCount => slots.Count(slot => slot != null);

        public WeaponBundle[] Slots => slots;
        private WeaponBundle[] slots;

        public event WeaponSlotUpdatedDelegate SlotUpdated;
        public event WeaponSlotMergedDelegate SlotMerged;

        private void Awake() {
            Instance = this;

            slots = new WeaponBundle[SlotCount];
        }


        public void Clear() {
            for (int i = 0; i < slots.Length; ++i) {
                if (slots[i] == null) continue;

                slots[i] = null;
            }

            SortSlots();
        }

        public AddWeaponResult AddWeapon(WeaponType weaponType, float boughtPrice = 0f) {
            // 인벤토리에 동일한 등급의 같은 무기가 2개 있으면 랭크업
            if (MergeWeapon(weaponType, 1, MergeSourceCount - 1, boughtPrice))
                return new AddWeaponResult(true, null);

            // 아닌 경우 개별 아이템으로 하나 획득
            int emptySlotIndex = GetEmptySlotIndex();

            if (emptySlotIndex == -1) return new AddWeaponResult(false, null);
            WeaponBundle slot = new(WeaponResource.Instance.library.GetData(weaponType));
            slot.SetBoughtPrice(boughtPrice);
            slots[emptySlotIndex] = slot;

            SortSlots();

            return new AddWeaponResult(true, slot);
        }

        public bool RemoveWeapon(WeaponBundle slot) {
            for (int i = 0; i < slots.Length; ++i) {
                if (slots[i] == slot) {
                    RemoveWeapon(i);

                    SortSlots();
                    return true;
                }
            }

            return false;
        }

        public void RemoveWeapon(WeaponType weaponType) {
            for (int i = 0; i < slots.Length; ++i) {
                if (slots[i] == null || slots[i].weapon.weaponType != weaponType) continue;

                WeaponBundle slot = slots[i];
                RuntimeWeaponManager.Instance.RemoveWeapon(slot);
                slots[i] = null;
            }

            SortSlots();
        }

        public bool RemoveWeapon(int index, bool withSort = true) {
            if (index < 0 || index >= slots.Length) return false;
            if (slots[index] == null) return false;

            WeaponBundle slot = slots[index];
            RuntimeWeaponManager.Instance.RemoveWeapon(slot);
            slots[index] = null;

            if (withSort)
                SortSlots();

            return true;
        }

        public bool MergeWeapon(WeaponType weaponType, int level, int needCount, float bonusBoughtPrice = 0f) {
            float boughtPrice = bonusBoughtPrice;

            IEnumerable<WeaponBundle> sameSlots = slots.Where((weaponBundle) => {
                return weaponBundle != null && weaponBundle.weapon.weaponType == weaponType &&
                       weaponBundle.level == level;
            });
            if (sameSlots.Count() > needCount)
                sameSlots = sameSlots.Take(needCount);

            if (sameSlots.Count() < needCount)
                return false;

            // 머지 조건에 부합
            // 통계 이전
            float accDamage = 0f;
            foreach (WeaponBundle sameSlot in sameSlots) {
                accDamage += sameSlot.accDamage;
            }

            // 무기 제거
            for (int i = 0; i < slots.Length; ++i) {
                foreach (WeaponBundle sameSlot in sameSlots) {
                    if (slots[i] == sameSlot) {
                        boughtPrice += sameSlot.BoughtPrice;
                        RemoveWeapon(i, false);
                        break;
                    }
                }
            }

            int emptySlotIndex = GetEmptySlotIndex();
            WeaponBundle newWeaponBundle = new(WeaponResource.Instance.library.GetData(weaponType));
            newWeaponBundle.level = level + 1;
            newWeaponBundle.SetBoughtPrice(boughtPrice);
            slots[emptySlotIndex] = newWeaponBundle;

            newWeaponBundle.accDamage = accDamage;

            SortSlots();

            int newWeaponIndex = Array.IndexOf(slots, newWeaponBundle);

            MergeWeaponInSlots();

            if (slots.Contains(newWeaponBundle))
                SlotMerged?.Invoke(newWeaponIndex, newWeaponBundle);

            SfxPlayer.Play("weapon.merge");

            GameAnalytics.LogEvent("WeaponMerged", //
                new Parameter("name", newWeaponBundle.weapon.name), //
                new Parameter("weaponLevel", newWeaponBundle.level), //
                new Parameter("stage", GameManager.Instance.PlayingGame.CurrentStage.name) //
            );
            return true;
        }

        public bool MergeWeaponInSlots() {
            bool merged = false;
            for (int i = 0; i < slots.Length; ++i) {
                WeaponBundle slot = slots[i];

                if (slot == null || slot.level >= WeaponBundle.MaxLevel) continue;

                merged |= MergeWeapon(slot.weapon.weaponType, slot.level, MergeSourceCount);
            }

            if (merged)
                MergeWeaponInSlots();
            return merged;
        }

        private void SortSlots() {
            List<WeaponBundle> existsWeaponList = slots.Where(x => x != null && x.weapon != null).ToList();

            existsWeaponList.Sort((left, right) => {
                if (left == null)
                    return 1;
                else if (right == null)
                    return -1;

                if (left.level != right.level)
                    return left.level.CompareTo(right.level) * -1;
                else
                    return left.weapon.weaponType.CompareTo(right.weapon.weaponType);
            });
            string log = "";
            foreach (WeaponBundle bundle in existsWeaponList) {
                if (bundle == null)
                    log += "(NULL), ";
                else
                    log += bundle.weapon.weaponType.ToString() + ", ";
            }

            for (int slotI = 0; slotI < slots.Length; ++slotI) {
                if (slotI < existsWeaponList.Count)
                    slots[slotI] = existsWeaponList[slotI];
                else
                    slots[slotI] = null;
            }

            SlotUpdated?.Invoke();
        }

        private int GetEmptySlotIndex() {
            for (int i = 0; i < slots.Length; ++i) {
                if (slots[i] == null) return i;
            }

            return -1;
        }
    }
}