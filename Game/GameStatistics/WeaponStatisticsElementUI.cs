using ArcaneSurvivorsClient.Locale;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class WeaponStatisticsElementUI : MonoBehaviour {
        public WeaponSlotUI slotUI;
        public LocaleText nameText;
        public TextMeshProUGUI damageText;
        public Image damageGuage;

        public WeaponBundle model;

        public void SetModel(WeaponBundle model) {
            this.model = model;

            slotUI.SetModel(model);
            nameText.Key = model.weapon.name;
            damageText.text = model.accDamage.ToDisplayString(DisplayNumberType.MetricPrefix);
        }

        public void SetGuage(float value) {
            damageGuage.fillAmount = value;
        }
    }
}