using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class WeaponStatisticsUI : MonoBehaviour {
        public Transform statArea;
        public GameObject statPrefab;

        public void UpdateUI() {
            statArea.ClearChilds();

            List<WeaponStatisticsElementUI> statUIList = new();

            WeaponBundle[] slots = WeaponInventory.Instance.Slots.Where(x => x != null).OrderBy(x => x.accDamage)
                .ToArray();
            float maxAccDamage = 0f;
            foreach (WeaponBundle weaponBundle in slots) {
                if (weaponBundle == null) continue;

                GameObject stat = statPrefab.Instantiate(statArea);
                WeaponStatisticsElementUI statUI = stat.GetComponent<WeaponStatisticsElementUI>();
                statUI.SetModel(weaponBundle);
                statUIList.Add(statUI);

                if (maxAccDamage < weaponBundle.accDamage)
                    maxAccDamage = weaponBundle.accDamage;
            }

            statUIList.Sort((left, right) => {
                return -left.model.accDamage.CompareTo(right.model.accDamage);
            });

            foreach (WeaponStatisticsElementUI statUI in statUIList) {
                float value;
                if (maxAccDamage > 0)
                    value = statUI.model.accDamage / maxAccDamage;
                else
                    value = 0f;

                statUI.SetGuage(value);
                statUI.transform.SetAsLastSibling();
            }
        }
    }
}