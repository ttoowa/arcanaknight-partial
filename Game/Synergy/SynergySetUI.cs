using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient.Game {
    public class SynergySetUI : MonoBehaviour {
        public RectTransform synergyArea;

        private void Start() {
            SynergySet.Instance.Updated += OnModelUpdated;
        }

        private void OnModelUpdated(SynergyBundle[] synergies) {
            synergyArea.ClearChilds();

            foreach (SynergyBundle synergy in synergies) {
                SynergyBundleUI synergyBundleUI = SynergyResource.Instance.synergyBundlePrefab.Instantiate(synergyArea)
                    .Get<SynergyBundleUI>();

                synergyBundleUI.SetModel(synergy);
            }
        }
    }
}