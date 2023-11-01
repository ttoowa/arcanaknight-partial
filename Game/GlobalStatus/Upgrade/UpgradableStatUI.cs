using System;
using System.Collections;
using ArcaneSurvivorsClient.Locale;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class UpgradableStatUI : MonoBehaviour {
        public GameObject elementPrefab;
        public RectTransform elementArea;

        public Button resetLevelButton;

        private void Start() {
            StartCoroutine(InitRoutine());

            resetLevelButton.onClick.AddListener(() => {
                ConfirmDialog.Show("dialog.resetReconfirm".ToLocale(), () => {
                    UpgradableStatManager.Instance.ResetLevels();
                });
            });
        }

        private IEnumerator InitRoutine() {
            yield return null;

            CreateElements();
        }

        private void CreateElements() {
            elementArea.ClearChilds();
            for (int i = 0; i < UpgradableStatManager.Instance.StatBundles.Length; ++i) {
                UpgradableStatBundle bundle = UpgradableStatManager.Instance.StatBundles[i];
                GameObject instance = elementPrefab.Instantiate(elementArea);
                UpgradableStatElementUI statElementUI = instance.Get<UpgradableStatElementUI>();

                statElementUI.SetModel(bundle);
            }
        }
    }
}