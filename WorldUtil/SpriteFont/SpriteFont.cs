using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    [CreateAssetMenu(fileName = "SpriteFont", menuName = "ScriptableObject/SpriteFont", order = 1)]
    public class SpriteFont : ScriptableObject {
        public SpriteFontCharacter[] characters;

        private readonly Dictionary<char, SpriteFontCharacter> characterDict = new();

        private bool isIndexed;

        public void Indexing(bool force = false) {
            if (isIndexed && !force) return;
            isIndexed = true;

            characterDict.Clear();

            foreach (SpriteFontCharacter character in characters) {
                if (characterDict.ContainsKey(character.character)) {
                    LogBuilder.Log(LogType.Error, nameof(SpriteFont), "Character already exists.",
                        new LogElement("Character", character.character.ToString()));
                    continue;
                }

                characterDict.Add(character.character, character);
            }
        }

        public Sprite GetSprite(char character) {
            Indexing();

            if (!characterDict.ContainsKey(character)) {
                Indexing(true);
                if (!characterDict.ContainsKey(character)) {
                    LogBuilder.Log(LogType.Error, nameof(SpriteFont), "Character not found.",
                        new LogElement("Character", character.ToString()));
                    return null;
                }
            }

            return characterDict[character].sprite;
        }
    }
}