using System;
using System.Collections;
using System.Collections.Generic;
using ArcaneSurvivorsClient.Locale;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    [RequireComponent(typeof(Button))]
    public class WeaponGoodsUI : MonoBehaviour, IShopGoodsUI {
        public IShopGoods Model => model;

        public LocaleText nameText;
        public LocaleText descText;
        public TextMeshProUGUI priceText;
        public Image iconImage;
        public RectTransform synergyArea;
        public GameObject goodsContent;
        public GameObject soldOutContent;

        private WeaponGoods model;

        public event Action Clicked;

        private void Awake() {
            GetComponent<Button>().onClick.AddListener(() => {
                if (model.IsSoldOut) return;

                Clicked?.Invoke();
            });

            OnSoldOutChanged(false);
        }

        private void OnDestroy() {
            if (model == null) return;

            model.SoldOutChanged -= OnSoldOutChanged;
        }

        public void SetModel(WeaponGoods model) {
            this.model = model;

            nameText.Key = model.Weapon.name;
            descText.Key = model.Weapon.description;
            iconImage.sprite = model.Weapon.GetIcon(0);
            priceText.text = WeaponShop.Instance.WeaponPrice.ToString();

            synergyArea.ClearChilds();
            foreach (SynergyType synergy in model.Weapon.synergies) {
                SynergyBadgeUI synergyBadge = SynergyResource.Instance.synergyBadgePrefab.Instantiate(synergyArea)
                    .Get<SynergyBadgeUI>();
                synergyBadge.SetModel(SynergyResource.Instance.library.GetData(synergy));
            }

            model.SoldOutChanged += OnSoldOutChanged;
        }

        private void OnSoldOutChanged(bool isSoldOut) {
            goodsContent.SetActive(!isSoldOut);
            soldOutContent.SetActive(isSoldOut);
        }
    }
}