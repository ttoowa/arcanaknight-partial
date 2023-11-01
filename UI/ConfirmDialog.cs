using System;
using ArcaneSurvivorsClient.Locale;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient {
    public class ConfirmDialog : MonoBehaviour {
        public TextMeshProUGUI contentText;
        public TextMeshProUGUI confirmText;
        public TextMeshProUGUI cancelText;

        public Button confirmButton;
        public Button cancelButton;

        public static void Show(string content, Action confirmAction) {
            Show(content, null, null, confirmAction, null);
        }

        public static void Show(string content, string confirmText, string cancelText, Action confirmAction,
            Action cancelAction) {
            ConfirmDialog dialog = DialogResource.Instance.confirmDialogPrefab
                .Instantiate(DialogResource.Instance.dialogArea).Get<ConfirmDialog>();

            dialog.contentText.text = content;
            if (confirmText != null)
                dialog.confirmText.text = confirmText;
            else
                dialog.confirmText.text = "keyword.confirm".ToLocale();

            if (cancelText != null)
                dialog.cancelText.text = cancelText;
            else
                dialog.cancelText.text = "keyword.cancel".ToLocale();

            dialog.confirmButton.onClick.AddListener(() => {
                confirmAction?.Invoke();
                dialog.gameObject.Destroy();
            });
            dialog.cancelButton.onClick.AddListener(() => {
                cancelAction?.Invoke();
                dialog.gameObject.Destroy();
            });
        }

        private void Awake() {
            FadeStack.Push();
        }

        private void OnDestroy() {
            FadeStack.Pop();
        }
    }
}