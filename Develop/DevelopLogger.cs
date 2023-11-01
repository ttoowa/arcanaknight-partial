using TMPro;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class DevelopLogger : MonoBehaviour {
        public static DevelopLogger Instance { get; private set; }

        [SerializeField]
        private TextMeshProUGUI logText;

        private void Awake() {
            Instance = this;
        }

        public static void Log(string message) {
            Instance.logText.text = message;
        }
    }
}