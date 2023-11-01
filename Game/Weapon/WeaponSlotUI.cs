using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class WeaponSlotUI : MonoBehaviour {
        public bool HasModel => model != null;

        [HideInInspector]
        public WeaponBundle model;

        public GameObject slot;
        public GameObject content;
        public GameObject synergySrcFX;
        public Image slotImage;
        public Image iconImage;

        public Transform starArea;
        public GameObject starPrefab;

        public Sprite[] slotSprites;

        public EventTrigger eventTrigger;

        public bool enableDrag = true;

        private RectTransform draggingShadow;

        private void Awake() {
            SetSynergyFXVisible(false);
        }

        private void Start() {
            AddDragEventListeners();
        }

        private void AddDragEventListeners() {
            EventTrigger.Entry pointerDownEntry = new();
            pointerDownEntry.eventID = EventTriggerType.PointerDown;
            pointerDownEntry.callback.AddListener((BaseEventData data) => {
                PointerEventData pointerData = data as PointerEventData;

                if (model == null || !enableDrag) return;

                OnShadowPointerDown(pointerData);
                OnShadowDrag(pointerData);
            });

            EventTrigger.Entry dragMoveEntry = new();
            dragMoveEntry.eventID = EventTriggerType.Drag;
            dragMoveEntry.callback.AddListener((BaseEventData data) => {
                PointerEventData pointerData = data as PointerEventData;

                if (!enableDrag) return;

                OnShadowDrag(pointerData);
            });

            EventTrigger.Entry pointerUpEntry = new();
            pointerUpEntry.eventID = EventTriggerType.PointerUp;
            pointerUpEntry.callback.AddListener((BaseEventData data) => {
                PointerEventData pointerData = data as PointerEventData;

                if (!enableDrag) return;

                OnShadowPointerUp(pointerData);
            });

            eventTrigger.triggers.Add(pointerDownEntry);
            eventTrigger.triggers.Add(dragMoveEntry);
            eventTrigger.triggers.Add(pointerUpEntry);
        }

        public void SetModel(WeaponBundle model) {
            this.model = model;

            if (model != null) {
                iconImage.gameObject.SetActive(true);
                iconImage.sprite = model.ActualIcon;
                slotImage.sprite = slotSprites[0];
                SetStarLevel(model.level);
            } else {
                iconImage.gameObject.SetActive(false);
                iconImage.sprite = null;
                slotImage.sprite = slotSprites[1];
            }
        }

        public void SetStarLevel(int level) {
            starArea.ClearChilds();
            for (int i = 0; i < level; ++i) {
                starPrefab.Instantiate(starArea, Vector3.zero);
            }

            if (level > 0) {
                iconImage.material =
                    WeaponResource.Instance.weaponMaterials[Mathf.Clamp(level - 1, 0, WeaponBundle.MaxLevel - 1)];
            }
        }

        public void SetSynergyFXVisible(bool accent) {
            synergySrcFX.SetActive(accent);
        }

        private void SetContentVisible(bool visible) {
            content.SetActive(visible);
        }

        private void OnShadowPointerDown(PointerEventData pointerData) {
            if (draggingShadow != null) return;

            draggingShadow = gameObject.gameObject.InstantiateFX(null, true, removeScripts: true)
                .GetComponent<RectTransform>();
            draggingShadow.SetParent(GlobalUI.Instance.topFXArea);
            draggingShadow.anchorMin = Vector2.zero;
            draggingShadow.anchorMax = Vector2.zero;
            draggingShadow.sizeDelta = slot.GetComponent<RectTransform>().rect.size;
            draggingShadow.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
            SetContentVisible(false);

            WeaponShopUI.Instance.SetSellContextVisible(true, model);
        }

        private void OnShadowDrag(PointerEventData pointerData) {
            if (draggingShadow == null) return;

            Vector2 cursorCanvasPosition =
                UIUtility.ScreenToCanvasPoint(GlobalUI.Instance.canvasTrsf_UI, pointerData.position);
            draggingShadow.anchoredPosition = cursorCanvasPosition;

            WeaponShopUI.Instance.SetSellActionFXVisible(
                WeaponShopUI.Instance.sellContext.panel.IsContainsScreenPoint(pointerData.position));
        }

        private void OnShadowPointerUp(PointerEventData pointerData) {
            if (draggingShadow == null) return;
            Destroy(draggingShadow.gameObject);

            SetContentVisible(true);
            WeaponShopUI.Instance.SetSellContextVisible(false);
            WeaponShopUI.Instance.SetSellActionFXVisible(false);

            if (WeaponShopUI.Instance.sellContext.panel.IsContainsScreenPoint(pointerData.position))
                WeaponShop.Instance.Sell(model);
        }
    }
}