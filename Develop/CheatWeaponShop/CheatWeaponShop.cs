using System;
using ArcaneSurvivorsClient.Game;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient {
    public class CheatWeaponShop : MonoBehaviour {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        public bool IsVisible => shopContext.activeSelf;

        public Button button;

        public GameObject shopContext;

        public GameObject goodsPrefab;
        public GameObject goodsArea;

        private void Start() {
            Weapon[] weapons = WeaponResource.Instance.library.dataObjects;

            // Create weapon goods
            goodsArea.ClearChilds();
            foreach (Weapon weapon in weapons) {
                Weapon weaponInstance = weapon;
                GameObject goods = goodsPrefab.Instantiate(goodsArea.transform);
                goods.Get<CheatWeaponGoods>().SetModel(weaponInstance);
                goods.Get<Button>().onClick.AddListener(() => {
                    WeaponInventory.Instance.AddWeapon(weaponInstance.weaponType);
                });
            }

            // Add button event listener
            button.onClick.AddListener(() => {
                SetVisible(!IsVisible);
            });

            SetVisible(false);
        }

        public void SetVisible(bool visible) {
            shopContext.SetActive(visible);
        }
#endif
    }
}