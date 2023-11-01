using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient.Game {
    public class GameResource : MonoBehaviour, ISingletone {
        public static GameResource Instance { get; private set; }

        [Header("UI")]
        public DayPhaseUI dayPhaseUI;

        public NightPhaseUI nightPhaseUI;

        public WeaponInventoryUI weponInventoryDayUI;

        public GameObject playerPawnStatusPrefab;

        public Sprite potionSprite;

        [Header("World")]
        public Transform playerArea;

        public Transform monsterArea;

        public GameObject fieldDropGoldPrefab;
        public GameObject fieldDropPotionPrefab;

        public GameObject pickupGoldUIFXPrefab;

        public ObjectFollower rotationIndicator;


        private void Awake() {
            Instance = this;
        }

        private void Start() {
            weponInventoryDayUI.AttachModel(WeaponInventory.Instance);
        }

        private void OnDestroy() {
            weponInventoryDayUI.DetachModel();
        }
    }
}