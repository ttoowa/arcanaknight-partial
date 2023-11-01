using System;
using ArcaneSurvivorsClient.Analytics;
using ArcaneSurvivorsClient.Locale;
using Firebase.Analytics;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class UpgradableStatBundle {
        public delegate void LevelChangedDelegate(int level, UpgradableStatBundle statBundle);

        public StatValue CurrentValue =>
            new(Define.totalValue.value * ((float)level / Define.maxLevel), Define.totalValue.type,
                Define.totalValue.benefitDirection);

        public int CurrentPrice => Define.GetPrice(level);

        public bool CanUpgrade => level < Define.maxLevel && GlobalStatus.Instance.SP.Value >= CurrentPrice;

        public readonly UpgradableStat Define;
        public int level;

        public event LevelChangedDelegate LevelChanged;

        public UpgradableStatBundle(UpgradableStat define) {
            Define = define;
            level = 0;
        }

        public JObject ToJObject() {
            return new JObject {
                ["level"] = level
            };
        }

        public void LoadFromJObject(JObject jStatBundle) {
            if (jStatBundle == null) return;

            level = jStatBundle.Value<int>("level");

            LevelChanged?.Invoke(level, this);
        }

        public void LevelUp() {
            if (!CanUpgrade) {
                if (GlobalStatus.Instance.SP.Value < CurrentPrice)
                    ToastMessage.Show("alert.needMoreSp".ToLocale());
                return;
            }

            int price = CurrentPrice;
            if (!GlobalStatus.Instance.SubtractSP(price)) return;
            UpgradableStatManager.Instance.AddUsedSp(price);

            ++level;

            LevelChanged?.Invoke(level, this);

            SaveData.MarkAsDirty();

            GameAnalytics.LogEvent("WeaponMerged", //
                new Parameter("name", Define.name), //
                new Parameter("price", price));
        }

        public void SetLevel(int level) {
            this.level = level;

            LevelChanged?.Invoke(level, this);
        }
    }
}