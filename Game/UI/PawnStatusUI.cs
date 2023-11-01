using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    [RequireComponent(typeof(WorldUI))]
    public class PawnStatusUI : MonoBehaviour {
        [SerializeField]
        private GuageUI hpGuage;

        private WorldUI worldUI;

        private void Awake() {
            worldUI = GetComponent<WorldUI>();
        }

        public void SetHP(float normalizedHP) {
            hpGuage.SetValue(Mathf.Clamp01(normalizedHP));
        }

        public void SetVisible(bool visible) {
            worldUI.IsActive = visible;
            gameObject.SetActive(visible);
        }
    }
}