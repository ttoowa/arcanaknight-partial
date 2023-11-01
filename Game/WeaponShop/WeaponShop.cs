using System;
using System.Collections.Generic;
using System.Linq;
using ArcaneSurvivorsClient.Analytics;
using ArcaneSurvivorsClient.Locale;
using Firebase.Analytics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ArcaneSurvivorsClient.Game {
    public class WeaponShop : MonoBehaviour {
        public struct GoodsGroup {
            public PotionGoods potion;
        }

        public delegate void WeaponGoodsUpdatedDelegate(IReadOnlyList<WeaponGoods> weaponGoods);

        public delegate void EtcGoodsUpdatedDelegate(GoodsGroup goodsGroup);

        public static WeaponShop Instance { get; private set; }

        private readonly List<WeaponGoods> weaponGoodsList = new();
        private PotionGoods potionGoods;

        // public GoodsPrice WeaponPrice = new(20, 0.2f);
        // public GoodsPrice RerollPrice = new(10, 0.2f);
        // public GoodsPrice PotionPrice = new(20, 0.2f);

        private static GameBalance GameBalance => GameManager.Instance.PlayingGame.PlayingChapter.gameBalance;

        private static GameDifficulty Difficulty =>
            GameManager.Instance.PlayingGame.PlayingChapter.difficultyState.SelectedDifficulty;

        public int WeaponPrice =>
            StatCalculator.GetShopPrice(
                GameBalance.weaponPriceRange.Sample(GameManager.Instance.PlayingGame.NormalizedStageNum));

        public int PotionPrice =>
            StatCalculator.GetShopPrice(
                GameBalance.potionPriceRange.Sample(GameManager.Instance.PlayingGame.NormalizedStageNum));

        public int RerollPrice =>
            StatCalculator.GetShopPrice(
                GameBalance.rerollPriceRange.Sample(GameManager.Instance.PlayingGame.NormalizedStageNum));

        public event WeaponGoodsUpdatedDelegate WeaponGoodsUpdated;

        public event EtcGoodsUpdatedDelegate EtcGoodsUpdated;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            WeaponGoodsUpdated?.Invoke(null);
        }

        public void RerollWeaponGoods(bool forFree = false, int weaponCount = 2) {
            if (forFree || GameManager.Instance.PlayingGame.SubtractGold(RerollPrice)) {
                weaponGoodsList.Clear();

                for (int i = 0; i < weaponCount; ++i) {
                    WeaponGoods weaponGoods = new(WeaponResource.Instance.library.GetRandomKey(), WeaponPrice);
                    weaponGoodsList.Add(weaponGoods);
                }

                WeaponGoodsUpdated?.Invoke(weaponGoodsList);

                // Collect statistics
                if (!forFree) {
                    GameManager.Instance.PlayingGame?.Statistics.CollectReroll();

                    SfxPlayer.Play("shop.reroll");

                    GameAnalytics.LogEvent("ShopRerolled", // 
                        new Parameter("price", RerollPrice),
                        new Parameter("stage", GameManager.Instance.PlayingGame.CurrentStage.name));
                }
            }
        }

        public void RefillPotionGoods() {
            potionGoods = new PotionGoods(PotionPrice);

            OnEtcGoodsUpdated();
        }

        public bool Buy(IShopGoods goods) {
            if (goods == null) return false;

            // Find price
            int price = 0;
            if (goods is WeaponGoods)
                price = WeaponPrice;
            else if (goods is PotionGoods)
                price = PotionPrice;
            else {
                LogBuilder.BuildException(LogType.Error, nameof(WeaponShop),
                    $"Unknown Goods type '{goods.GetType().Name}'.");
                return false;
            }


            if (GameManager.Instance.PlayingGame.Gold >= price) {
                if (goods is WeaponGoods) {
                    WeaponInventory.AddWeaponResult result =
                        WeaponInventory.Instance.AddWeapon((goods as WeaponGoods).Weapon.weaponType, price);

                    if (!result.result) {
                        ToastMessage.Show("shop.msg.insufficientInventorySpace".ToLocale());
                        return false;
                    }

                    GameManager.Instance.PlayingGame.SubtractGold(price);
                    GameManager.Instance.PlayingGame?.Statistics.CollectWeaponPurchased();

                    GameAnalytics.LogEvent("WeaponBought", // 
                        new Parameter("name", (goods as WeaponGoods).Weapon.name), // 
                        new Parameter("price", WeaponPrice),
                        new Parameter("stage", GameManager.Instance.PlayingGame.CurrentStage.name));
                } else if (goods is PotionGoods) {
                    if (GamePlayer.LocalPlayer.Pawn.IsFullHp) {
                        ToastMessage.Show("shop.msg.alreadyFullHp".ToLocale());
                        return false;
                    }

                    GamePlayer.LocalPlayer.Pawn.HealFull();
                    GameManager.Instance.PlayingGame.SubtractGold(price);

                    GameAnalytics.LogEvent("PotionBought", // 
                        new Parameter("price", PotionPrice),
                        new Parameter("stage", GameManager.Instance.PlayingGame.CurrentStage.name));
                }

                goods.SetSoldOut(true);
                SfxPlayer.Play("shop.buy");
                return true;
            } else {
                ToastMessage.Show("shop.msg.notEnoughGold".ToLocale());
                return false;
            }
        }

        public bool Sell(WeaponBundle slot) {
            if (WeaponInventory.Instance.WeaponCount <= 1) {
                ToastMessage.Show("shop.msg.cantSellLastWeapon".ToLocale());
                return false;
            }

            if (WeaponInventory.Instance.RemoveWeapon(slot)) {
                GameManager.Instance.PlayingGame.AddGold(slot.SellPrice);
                SfxPlayer.Play("shop.sell");
            }

            GameAnalytics.LogEvent("WeaponSold", //
                new Parameter("name", slot.weapon.name), //
                new Parameter("sellPrice", slot.SellPrice), //
                new Parameter("weaponLevel", slot.level), //
                new Parameter("stage", GameManager.Instance.PlayingGame.CurrentStage.name) //
            );

            return true;
        }

        private void OnEtcGoodsUpdated() {
            EtcGoodsUpdated?.Invoke(new GoodsGroup {
                potion = potionGoods
            });
        }
    }
}