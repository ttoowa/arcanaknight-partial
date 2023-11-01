using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class WeaponInventoryUI : MonoBehaviour, ISingletone {
        public GameObject panel;
        public GameObject slotPrefab;
        public RectTransform slotArea;

        private WeaponInventory model;

        private WeaponSlotUI[] slotUIs;

        private bool renderFlag;

        private void Update() {
            if (renderFlag) {
                renderFlag = false;

                Render();
            }
        }

        public void SetVisible(bool visible) {
            panel.SetActive(visible);
        }

        public void AttachModel(WeaponInventory model) {
            this.model = model;
            model.SlotUpdated += OnSlotChanged;
            model.SlotMerged += OnSlotMerged;
        }

        public void DetachModel() {
            if (model == null) return;

            model.SlotUpdated -= OnSlotChanged;
            model.SlotMerged -= OnSlotMerged;
            model = null;
        }

        // TODO : 이 함수 작업하기. WeaponInventoryUI 참조를 어떻게 제공할지도 고민해봐야함
        public void SetSynergySrcVisible(bool visible, SynergyType? synergyType = null) {
            foreach (WeaponSlotUI slotUI in slotUIs) {
                if (slotUI == null || !slotUI.HasModel) continue;

                if (visible && synergyType.HasValue && slotUI.model.weapon.synergies.Contains(synergyType.Value))
                    slotUI.SetSynergyFXVisible(true);
                else
                    slotUI.SetSynergyFXVisible(false);
            }
        }

        private void OnSlotChanged() {
            renderFlag = true;
        }

        private void OnSlotMerged(int slotIndex, WeaponBundle slot) {
            Vector3 targetPosition = GetSlotWorldPosition(slotIndex, -1f);
            FXResource.Instance.weaponMergeFXPrefab.InstantiateFX(targetPosition, false, 3f);
        }

        private void Render() {
            CreateSlots();
        }

        private void CreateSlots() {
            slotArea.transform.ClearChilds();
            slotUIs = new WeaponSlotUI[model.Slots.Length];

            if (model == null) return;

            for (int i = 0; i < model.Slots.Length; ++i) {
                WeaponBundle slot = model.Slots[i];
                GameObject slotUIGObj = Instantiate(slotPrefab, slotArea);
                WeaponSlotUI slotUI = slotUIGObj.GetComponent<WeaponSlotUI>();
                slotUIs[i] = slotUI;

                if (slot == null) {
                    slotUI.SetModel(null);
                    slotUI.SetStarLevel(-1);
                    continue;
                }

                slotUI.SetModel(slot);
                slotUI.SetStarLevel(slot.level);
            }
        }

        public Vector3 GetSlotWorldPosition(int index, float offsetZ) {
            if (slotUIs == null) CreateSlots();
            return slotUIs[index].transform.position + new Vector3(0f, 0f, offsetZ);
        }
    }
}