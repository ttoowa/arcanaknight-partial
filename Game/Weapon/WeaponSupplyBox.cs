using System;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class WeaponSupplyBox : MonoBehaviour {
        public WeaponType weaponType;

        public SpriteRenderer iconRenderer;

        private void Start() {
            Weapon weapon = WeaponResource.Instance.library.GetData(weaponType);
            iconRenderer.sprite = weapon.GetIcon(1);
        }

        public void Destroy() {
            Destroy(gameObject);
        }
    }
}