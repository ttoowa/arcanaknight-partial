using System;
using System.Collections.Generic;
using ArcaneSurvivorsClient.Locale;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class PlayableCharacterSelectorUI : MonoBehaviour {
        public struct UIEntry {
            public PlayableCharacterType type;
            public PlayableCharacterCardUI cardUI;
            public PlayableCharacterSlotUI slotUI;

            public UIEntry(PlayableCharacterType type, PlayableCharacterCardUI cardUI, PlayableCharacterSlotUI slotUI) {
                this.type = type;
                this.cardUI = cardUI;
                this.slotUI = slotUI;
            }
        }

        private const int MinSlotCount = 4;

        public GameObject cardArea;
        public GameObject slotArea;

        public GameObject cardPrefab;
        public GameObject slotPrefab;

        public LocaleText selectedCharacterDescText;

        private readonly Dictionary<PlayableCharacterType, UIEntry> uiEntryDict = new();

        private void Awake() {
        }

        private void Start() {
            CreateCharacterCards();

            PlayableCharacterManager.Instance.CharacterSelected += OnCharacterSelected;
            PlayableCharacterManager.Instance.AvailableCharacterChanged += OnAvailableCharacterChanged;

            PlayableCharacterManager.Instance.SelectCharacter(PlayableCharacterType.Karin);
        }

        private void OnAvailableCharacterChanged(HashSet<PlayableCharacterType> availablecharacterset) {
            foreach (UIEntry entry in uiEntryDict.Values) {
                entry.slotUI.SetAvailable(availablecharacterset.Contains(entry.type));
            }
        }

        private void OnCharacterSelected(PlayableCharacterType character) {
            foreach (UIEntry entry in uiEntryDict.Values) {
                entry.slotUI.SetSelected(entry.type == character);
            }

            PlayableCharacter characterDefine = PlayableCharacterResource.Instance.library.GetData(character);
            selectedCharacterDescText.Parameters = new[] { $"{{{characterDefine.name}}}" };

            // TODO : Card Area Scrolling
        }

        private void CreateCharacterCards() {
            PlayableCharacter[] characters = PlayableCharacterResource.Instance.library.dataObjects;

            cardArea.ClearChilds();
            slotArea.ClearChilds();
            uiEntryDict.Clear();
            for (int i = 0; i < Mathf.Max(MinSlotCount, characters.Length); ++i) {
                GameObject slot = slotPrefab.Instantiate(slotArea.transform);
                PlayableCharacterSlotUI slotUI = slot.GetComponent<PlayableCharacterSlotUI>();

                PlayableCharacterCardUI cardUI = null;
                if (i < characters.Length) {
                    PlayableCharacter character = characters[i];

                    GameObject card = cardPrefab.Instantiate(cardArea.transform);
                    cardUI = card.GetComponent<PlayableCharacterCardUI>();
                    cardUI.SetModel(character);

                    slotUI.SetModel(character);
                    slotUI.button.onClick.AddListener(() => {
                        PlayableCharacterManager.Instance.SelectCharacter(character.type);
                    });

                    slotUI.SetSelected(PlayableCharacterManager.Instance.SelectedCharacterType == character.type);

                    uiEntryDict.Add(character.type, new UIEntry(character.type, cardUI, slotUI));
                    slotUI.SetAvailable(
                        PlayableCharacterManager.Instance.AvailableCharacterSet.Contains(character.type));
                } else
                    slotUI.SetAvailable(false);
            }
        }
    }
}