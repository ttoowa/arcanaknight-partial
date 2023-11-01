using ArcaneSurvivorsClient.Locale;
using TMPro;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class GameDifficultyEffectUI : MonoBehaviour {
        public LocaleText headerText;
        public TextMeshProUGUI valueText;

        /// <summary>
        ///     Normal, Positive, Negative
        /// </summary>
        public Color[] valueTextColors;

        public void SetModel(GameDifficultyEffect model) {
            headerText.Key = model.header;

            float statValue = model.statValue.ActualValue;
            valueText.text = statValue >= 0 ? "+" + model.statValue.ToString() : model.statValue.ToString();
            if (model.statValue.benefitDirection == StatValueBenefitDirection.Negative)
                statValue *= -1f;

            if (statValue == 0f)
                valueText.color = valueTextColors[0];
            else if (statValue > 0f)
                valueText.color = valueTextColors[1];
            else
                valueText.color = valueTextColors[2];
        }
    }
}