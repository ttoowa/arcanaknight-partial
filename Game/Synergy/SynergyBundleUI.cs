using System;
using ArcaneSurvivorsClient.Locale;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ArcaneSurvivorsClient.Game {
    [ExecuteInEditMode]
    public class SynergyBundleUI : MonoBehaviour {
        public ScrollableEventTrigger eventTrigger;
        public SynergyBundle model;
        public LocaleText nameText;
        public TextMeshProUGUI levelText;
        public SynergyBadgeUI badgeUI;
        public CanvasGroup canvasGroup;
        public bool showName = true;
        public bool showLevel = true;
        public bool showBuffApplied = true;
        public bool applyBuffColor;
        public SynergyBadgeSprite[] buffLevelSprites;


        private void Start() {
            if (showLevel) {
                EventTrigger.Entry pointerDownEntry = new();
                pointerDownEntry.eventID = EventTriggerType.PointerDown;
                pointerDownEntry.callback.AddListener((BaseEventData data) => {
                    if (model == null) return;

                    GameResource.Instance.weponInventoryDayUI.SetSynergySrcVisible(true, model.synergy.synergyType);
                    SynergyBuffDetailUI.Instance.SetVisible(true);
                    SynergyBuffDetailUI.Instance.SetModel(model);
                });

                EventTrigger.Entry pointerUpEntry = new();
                pointerUpEntry.eventID = EventTriggerType.PointerUp;
                pointerUpEntry.callback.AddListener((BaseEventData data) => {
                    GameResource.Instance.weponInventoryDayUI.SetSynergySrcVisible(false);
                    SynergyBuffDetailUI.Instance.SetVisible(false);
                });

                eventTrigger.triggers.Add(pointerDownEntry);
                eventTrigger.triggers.Add(pointerUpEntry);
            }
        }

        public void SetModel(SynergyBundle model) {
            this.model = model;
            badgeUI.SetModel(model.synergy);

            UpdateUI();
        }

        public void SetLevelVisible(bool visible) {
            showLevel = visible;
            levelText.gameObject.SetActive(visible);
        }

        public void UpdateUI() {
            nameText.Key = model.synergy.name;
            levelText.text = model.DisplayLevel;

            if (applyBuffColor) {
                int spriteIndex = Mathf.Clamp(model.ActualBuffLevel, 0, buffLevelSprites.Length - 1);
                SynergyBadgeSprite spriteResource = buffLevelSprites[spriteIndex];
                badgeUI.slotImage.sprite = spriteResource.badgeSprite;
                badgeUI.iconImage.color = spriteResource.iconColor;
            }

            if (showBuffApplied)
                canvasGroup.alpha = model.ActualBuffLevel > 0 ? 1 : 0.2f;
            else
                canvasGroup.alpha = 1f;
        }

#if UNITY_EDITOR
        private void Update() {
            if (levelText != null) {
                if (showLevel) {
                    if (levelText.gameObject.activeSelf == false)
                        levelText.gameObject.SetActive(true);
                } else {
                    if (levelText.gameObject.activeSelf)
                        levelText.gameObject.SetActive(false);
                }
            }

            if (nameText != null) {
                if (showName) {
                    if (nameText.gameObject.activeSelf == false)
                        nameText.gameObject.SetActive(true);
                } else {
                    if (nameText.gameObject.activeSelf)
                        nameText.gameObject.SetActive(false);
                }
            }
        }
#endif
    }
}