using System;
using ArcaneSurvivorsClient.Locale;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class PlayableCharacterCardUI : MonoBehaviour {
        public Image cardBackImage;
        public Image cardFrameImage;
        public Image illustImage;
        public LocaleText nameText;
        public LocaleText descText;
        public Image startingWeaponImage;

        [Header("Stats")]
        public StatRowUI hpStat;

        public StatRowUI hpRegenStat;
        public StatRowUI attackPowerStat;
        public StatRowUI attackSpeedStat;
        public StatRowUI moveSpeedStat;

        private StatRowUI[] statRowUIs;

        public void SetModel(PlayableCharacter model) {
            statRowUIs = new[] { hpStat, hpRegenStat, attackPowerStat, attackSpeedStat, moveSpeedStat };

            illustImage.sprite = model.cardIllust;
            nameText.Key = model.name;
            descText.Key = model.desc;
            startingWeaponImage.sprite = WeaponResource.Instance.library.GetData(model.startWeaponType).GetIcon(1);

            hpStat.valueText.text = $"{model.ability.hp.ToString("0.0")}%";
            hpRegenStat.valueText.text = $"{model.ability.hpRegen.ToString("0.0")}%";
            attackPowerStat.valueText.text = model.ability.DisplayDamageScale;
            attackSpeedStat.valueText.text = model.ability.DisplayAttackCooltimeScale;
            moveSpeedStat.valueText.text = model.ability.DisplayMoveSpeedScale;

            cardBackImage.color = model.themeColor;
            cardFrameImage.color = model.themeColor;

            Color statTitleColor = model.StatTitleColor;
            foreach (StatRowUI statRowUI in statRowUIs) {
                statRowUI.headerText.color = statTitleColor;
                statRowUI.iconImage.color = statTitleColor;
            }
        }
    }
}