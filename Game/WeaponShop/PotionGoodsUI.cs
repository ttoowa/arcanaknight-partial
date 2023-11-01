using System;
using ArcaneSurvivorsClient.Locale;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    [RequireComponent(typeof(Button))]
    public class PotionGoodsUI : MonoBehaviour, IShopGoodsUI {
        public IShopGoods Model => model;

        public LocaleText nameText;
        public LocaleText descText;
        public TextMeshProUGUI priceText;
        public GameObject goodsContent;
        public GameObject soldOutContent;

        private PotionGoods model;

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

        public void SetModel(PotionGoods model) {
            this.model = model;

            nameText.Key = model.name;
            descText.Key = model.description;
            priceText.text = WeaponShop.Instance.PotionPrice.ToString();

            model.SoldOutChanged += OnSoldOutChanged;
        }

        private void OnSoldOutChanged(bool isSoldOut) {
            goodsContent.SetActive(!isSoldOut);
            soldOutContent.SetActive(isSoldOut);
        }
    }
}