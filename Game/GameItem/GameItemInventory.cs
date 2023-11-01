using System;

namespace ArcaneSurvivorsClient.Game {
    /// <summary>
    ///     기획 변경으로 사용하지 않는 클래스입니다.
    /// </summary>
    [Obsolete]
    public class GameItemInventory {
        public delegate void SlotUpdatedDelegate(int slotIndex, GameItemBundle bundle);

        public const int SlotCount = 2;

        public GameItemBundle[] slots;

        public int EmptySlotCount {
            get {
                int count = 0;
                foreach (GameItemBundle slot in slots) {
                    if (slot == null)
                        ++count;
                }

                return count;
            }
        }

        public event SlotUpdatedDelegate SlotUpdated;
        public event SlotUpdatedDelegate SlotAmountChanged;

        public GameItemInventory() {
            slots = new GameItemBundle[SlotCount];
            ClearAllSlots();
        }

        public bool AddItem(GameItemBundle bundle) {
            bool handled = false;
            for (int i = 0; i < slots.Length; ++i) {
                GameItemBundle slot = slots[i];
                if (slot == null || slot.item.itemType != bundle.item.itemType)
                    continue;

                if (slot.amount + bundle.amount <= slot.item.maxStack) {
                    slot.AddAmount(bundle.amount);
                    SlotAmountChanged?.Invoke(i, slot);
                    return true;
                } else {
                    bundle.SetAmount(slot.amount + bundle.amount - slot.item.maxStack);
                    slot.SetAmount(slot.item.maxStack);
                    SlotAmountChanged?.Invoke(i, slot);
                    handled = true;
                }
            }

            for (int i = 0; i < slots.Length; ++i) {
                if (slots[i] != null)
                    return handled;

                if (bundle.item.maxStack >= bundle.amount) {
                    SetSlot(i, bundle.Clone());
                    SlotAmountChanged?.Invoke(i, slots[i]);
                    return true;
                } else {
                    GameItemBundle slot = bundle.Clone();
                    slot.SetAmount(slot.item.maxStack);
                    SetSlot(i, slot);
                    SlotAmountChanged?.Invoke(i, slot);
                    bundle.SetAmount(bundle.amount - slot.item.maxStack);
                }

                if (bundle.amount <= 0)
                    return true;
            }

            return handled;
        }

        public bool RemoveItem(GameItemType itemType, int amount = 1) {
            // Check if there is enough item.
            int itemCount = 0;
            for (int i = 0; i < slots.Length; ++i) {
                GameItemBundle slot = slots[i];
                if (slot == null || slot.item.itemType != itemType)
                    continue;

                itemCount += slot.amount;
            }

            if (itemCount < amount)
                return false;

            // Remove item.
            for (int i = 0; i < slots.Length; ++i) {
                GameItemBundle slot = slots[i];
                if (slot == null || slot.item.itemType != itemType)
                    continue;

                if (slot.amount >= amount) {
                    slot.AddAmount(-amount);
                    SlotAmountChanged?.Invoke(i, slot);
                    if (slot.amount <= 0)
                        ClearSlot(i);
                } else {
                    amount -= slot.amount;
                    ClearSlot(i);
                }
            }

            return true;
        }

        public bool ClearItem(GameItemType itemType) {
            bool handled = false;
            for (int i = 0; i < slots.Length; ++i) {
                if (slots[i] == null || slots[i].item.itemType != itemType)
                    continue;

                ClearSlot(i);
                handled = true;
            }

            return handled;
        }

        public void SetSlot(int slotIndex, GameItemBundle bundle) {
            slots[slotIndex] = bundle;

            SlotUpdated?.Invoke(slotIndex, bundle);
        }

        public void ClearSlot(int slotIndex) {
            slots[slotIndex] = null;

            SlotUpdated?.Invoke(slotIndex, null);
        }

        public void ClearAllSlots() {
            for (int i = 0; i < slots.Length; ++i) {
                ClearSlot(i);
            }
        }

        public int GetEmptySlotIndex() {
            for (int i = 0; i < slots.Length; ++i) {
                if (slots[i] == null)
                    return i;
            }

            return -1;
        }
    }
}