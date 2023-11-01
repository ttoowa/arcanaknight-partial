using System;
using System.Collections.Generic;
using ArcaneSurvivorsClient.Locale;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class WeaponShopUI : MonoBehaviour {
        [Serializable]
        public class BuyContext {
            public GameObject panel;

            public TextMeshProUGUI goodsNameText;
            public TextMeshProUGUI goodsDescText;
            public TextMeshProUGUI priceText;
            public Image iconImage;
            public RectTransform synergeArea;
            public Button cancelButton;
            public Button buyButton;
        }

        [Serializable]
        public class SellContext {
            public RectTransform panel;

            public GameObject actionFX;
            public TextMeshProUGUI priceText;
        }

        public static WeaponShopUI Instance { get; private set; }

        [HideInInspector]
        public WeaponShop model;

        public GameObject panel;
        public RectTransform goodsUIArea;
        public GameObject weaponGoodsUIPrefab;
        public GameObject potionGoodsUIPrefab;
        public Button exitShopButton;
        public Button rerollButton;
        public TextMeshProUGUI rerollPriceText;

        public BuyContext buyContext;
        public SellContext sellContext;

        private float weaponUIAnimTime;

        private WeaponGoodsUI[] weaponGoodsUIs;
        private PotionGoodsUI potionGoodsUI;

        private IShopGoods buyTargetGoods;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            model = WeaponShop.Instance;

            model.WeaponGoodsUpdated += OnWeaponGoodsUpdated;
            model.EtcGoodsUpdated += OnEtcGoodsUpdated;

            buyContext.buyButton.onClick.AddListener(() => {
                if (buyTargetGoods != null)
                    model.Buy(buyTargetGoods);
                SetBuyContextVisible(false);
            });
            buyContext.cancelButton.onClick.AddListener(() => {
                SetBuyContextVisible(false);
            });

            rerollButton.onClick.AddListener(() => {
                model.RerollWeaponGoods();
            });

            goodsUIArea.ClearChilds();

            SetVisible(false);
            SetBuyContextVisible(false);
            SetSellContextVisible(false);
        }

        private void Update() {
            weaponUIAnimTime += Time.deltaTime;

            if (weaponGoodsUIs != null) {
                for (int i = 0; i < weaponGoodsUIs.Length; ++i) {
                    WeaponGoodsUI goodsUI = weaponGoodsUIs[i];
                    if (goodsUI.gameObject.activeSelf) continue;

                    if (weaponUIAnimTime >= 0.06f * i)
                        goodsUI.gameObject.SetActive(true);
                }
            }
        }

        public void SetVisible(bool visible) {
            panel.SetActive(visible);
        }

        public void SetBuyContextVisible(bool visible, IShopGoods goods = null) {
            buyContext.panel.SetActive(visible);

            if (visible && goods != null) {
                buyContext.goodsNameText.text = goods.DisplayName;
                buyContext.goodsDescText.text = goods.DisplayDescription;
                buyContext.priceText.text =
                    model.WeaponPrice.ToDisplayString(DisplayNumberType.WithComma); //goods.Weapon.price.ToString();
                buyContext.iconImage.sprite = goods.Icon;
                buyContext.synergeArea.gameObject.SetActive(false);

                if (goods is WeaponGoods) {
                    WeaponGoods weaponGoods = goods as WeaponGoods;
                    buyContext.synergeArea.ClearChilds();
                    buyContext.synergeArea.gameObject.SetActive(true);
                    foreach (SynergyType synergy in weaponGoods.Weapon.synergies) {
                        SynergyBundleUI synergyBundleUI = SynergyResource.Instance.synergyBundlePrefab
                            .Instantiate(buyContext.synergeArea).GetComponent<SynergyBundleUI>();
                        synergyBundleUI.SetModel(new SynergyBundle(synergy));
                        synergyBundleUI.SetLevelVisible(false);
                    }
                }
            }
        }

        public void SetSellContextVisible(bool visible, WeaponBundle slot = null) {
            sellContext.panel.gameObject.SetActive(visible);

            if (visible && slot != null)
                sellContext.priceText.text = slot.SellPrice.ToString();
        }

        public void SetSellActionFXVisible(bool visible) {
            sellContext.actionFX.SetActive(visible);
        }

        private void OnWeaponGoodsUpdated(IReadOnlyList<WeaponGoods> goodsList) {
            ClearGoods();
            if (goodsList == null) return;

            rerollPriceText.text = model.RerollPrice.ToString();

            if (weaponGoodsUIs != null) {
                foreach (WeaponGoodsUI weaponGoodsUI in weaponGoodsUIs) {
                    Destroy(weaponGoodsUI.gameObject);
                }
            }

            List<WeaponGoodsUI> goodsUIList = new();
            for (int i = 0; i < goodsList.Count; ++i) {
                WeaponGoodsUI goodsUI = weaponGoodsUIPrefab.Instantiate(goodsUIArea).GetComponent<WeaponGoodsUI>();
                WeaponGoods goods = goodsList[i];
                goodsUI.SetModel(goodsList[i]);
                goodsUI.gameObject.SetActive(false);
                goodsUIList.Add(goodsUI);

                goodsUI.Clicked += () => {
                    OnClickGoods(goods);
                };
            }

            weaponGoodsUIs = goodsUIList.ToArray();

            weaponUIAnimTime = 0f;

            SortGoods();
        }

        private void OnEtcGoodsUpdated(WeaponShop.GoodsGroup goodsGroup) {
            if (potionGoodsUI != null)
                Destroy(potionGoodsUI.gameObject);

            potionGoodsUI = potionGoodsUIPrefab.Instantiate(goodsUIArea).GetComponent<PotionGoodsUI>();
            potionGoodsUI.SetModel(goodsGroup.potion);
            potionGoodsUI.Clicked += () => {
                OnClickGoods(goodsGroup.potion);
            };

            SortGoods();
        }

        private void SortGoods() {
            if (potionGoodsUI != null)
                potionGoodsUI.transform.SetAsLastSibling();

            if (weaponGoodsUIs != null) {
                for (int i = 0; i < weaponGoodsUIs.Length; ++i) {
                    weaponGoodsUIs[i].transform.SetAsLastSibling();
                }
            }
        }

        private void ClearGoods() {
            if (weaponGoodsUIs == null) return;

            foreach (WeaponGoodsUI goodsUI in weaponGoodsUIs) {
                Destroy(goodsUI.gameObject);
            }
        }

        private void OnClickGoods(IShopGoods goods) {
            if (goods == null) return;

            buyTargetGoods = goods;

            SetBuyContextVisible(true, goods);
        }
    }
}