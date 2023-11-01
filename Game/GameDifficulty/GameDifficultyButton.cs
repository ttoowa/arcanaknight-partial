using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class GameDifficultyButton : MonoBehaviour {
        public Button button;
        public GameObject content;
        public GameObject locked;
        public TextMeshProUGUI levelText;

        public Image contentPanelImage;
        public Sprite[] contentPanelSprites;

        [HideInInspector]
        public GameDifficulty model;

        public void SetModel(GameDifficulty model) {
            this.model = model;

            levelText.text = model.level.ToString();
        }

        public void SetAvailable(bool isAvailable) {
            content.SetActive(isAvailable);
            locked.SetActive(!isAvailable);
        }

        public void SetSelected(bool isSelected) {
            contentPanelImage.sprite = isSelected ? contentPanelSprites[0] : contentPanelSprites[1];
        }
    }
}