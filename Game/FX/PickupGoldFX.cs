using TMPro;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class PickupGoldFX : MonoBehaviour {
        public TextMeshProUGUI amountText;

        public void SetAmount(float amount) {
            amountText.text = $"+{Mathf.FloorToInt(amount)}";
        }
    }
}