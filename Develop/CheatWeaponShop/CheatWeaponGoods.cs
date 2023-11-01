using ArcaneSurvivorsClient.Game;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient {
    public class CheatWeaponGoods : MonoBehaviour {
        public Image iconImage;

        public void SetModel(Weapon model) {
            iconImage.sprite = model.GetIcon(1);
        }
    }
}