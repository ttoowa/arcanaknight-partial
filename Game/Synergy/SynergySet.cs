using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class SynergySet : MonoBehaviour {
        public delegate void UpdatedDelegate(SynergyBundle[] synergies);

        public static SynergySet Instance { get; private set; }

        private readonly List<SynergyBundle> synergyList = new();

        public event UpdatedDelegate Updated;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            WeaponInventory.Instance.SlotUpdated += OnSlotUpdated;

            UpdateSynergies();
        }

        private void OnDestroy() {
            WeaponInventory.Instance.SlotUpdated -= OnSlotUpdated;
        }

        private void OnSlotUpdated() {
            UpdateSynergies();
        }

        public void UpdateSynergies() {
            synergyList.Clear();

            HashSet<WeaponType> weaponTypeSet = new();
            Dictionary<SynergyType, SynergyBundle> synergyDict = new();
            foreach (WeaponBundle weaponSlot in WeaponInventory.Instance.Slots) {
                if (weaponSlot == null || weaponSlot.weapon == null) continue;

                if (weaponTypeSet.Contains(weaponSlot.weapon.weaponType)) continue;
                weaponTypeSet.Add(weaponSlot.weapon.weaponType);

                foreach (SynergyType synergy in weaponSlot.weapon.synergies) {
                    if (synergyDict.ContainsKey(synergy)) {
                        SynergyBundle bundle = synergyDict[synergy];

                        bundle.AddLevel(1);
                    } else {
                        SynergyBundle bundle = new(synergy);
                        bundle.SetLevel(1);
                        synergyDict.Add(synergy, bundle);
                    }
                }
            }

            foreach (KeyValuePair<SynergyType, SynergyBundle> pair in synergyDict) {
                synergyList.Add(pair.Value);
            }

            synergyList.Sort((left, right) => {
                int leftBuffLevel = left.ActualBuffLevel;
                int rightBuffLevel = right.ActualBuffLevel;
                int leftLevel = left.Level;
                int rightLevel = right.Level;

                if (leftBuffLevel == rightBuffLevel) {
                    if (leftLevel == rightLevel)
                        return left.synergy.synergyType.CompareTo(right.synergy.synergyType);
                    else
                        return leftLevel.CompareTo(rightLevel) * -1;
                } else
                    return leftBuffLevel.CompareTo(rightBuffLevel) * -1;
            });

            Updated?.Invoke(synergyList.ToArray());
        }
    }
}