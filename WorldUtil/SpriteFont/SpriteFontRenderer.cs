using System;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class SpriteFontRenderer : MonoBehaviour {
        public string Text {
            get => text;
            set {
                text = value;
                BuildSprites();
            }
        }

        public float FontScale {
            get => fontScale;
            set {
                fontScale = value;
                UpdateLayout();
            }
        }

        public float CharacterSpace {
            get => characterSpace;
            set {
                characterSpace = value;
                UpdateLayout();
            }
        }

        public Color Color {
            get => color;
            set {
                color = value;
                UpdateColor();
            }
        }

        [SerializeField]
        private SpriteFont font;

        [SerializeField]
        private float fontScale = 1f;

        [SerializeField]
        private float characterSpace = 0f;

        [SerializeField]
        private string text;

        [SerializeField]
        private Color color = Color.white;

        private SpriteRenderer[] spriteRenderers;

#if UNITY_EDITOR
        private string renderedText;
        private float renderedFontScale;
        private float renderedCharacterSpace;
        private Color renderedColor;

        private void OnDrawGizmos() {
            if (font == null) return;

            if (renderedText != text) {
                renderedText = text;

                font.Indexing(true);
                BuildSprites();
            }

            if (renderedFontScale != fontScale || renderedCharacterSpace != characterSpace) {
                renderedFontScale = fontScale;
                renderedCharacterSpace = characterSpace;

                font.Indexing(true);
                UpdateLayout();
            }

            if (renderedColor != color) {
                renderedColor = color;

                UpdateColor();
            }
        }
#endif

        public void BuildSprites() {
            transform.ClearChilds();
            spriteRenderers = null;

            if (font == null) return;

            spriteRenderers = new SpriteRenderer[text.Length];
            for (int i = 0; i < text.Length; ++i) {
                char character = text[i];
                Sprite sprite = font.GetSprite(character);

                GameObject spriteObject = new($"{character}");
                spriteObject.transform.SetParent(transform, false);
                SpriteRenderer spriteRenderer = spriteObject.Add<SpriteRenderer>();
                spriteRenderer.sprite = sprite;
                spriteRenderers[i] = spriteRenderer;
            }

            UpdateLayout();
        }

        public void UpdateLayout() {
            if (spriteRenderers == null) return;

            float characterPosition = 0f;
            float[] characterPositions = new float[spriteRenderers.Length];
            for (int i = 0; i < spriteRenderers.Length; ++i) {
                SpriteRenderer spriteRenderer = spriteRenderers[i];
                characterPositions[i] = characterPosition;
                spriteRenderer.transform.localScale = new Vector3(fontScale, fontScale, fontScale);
                characterPosition += (spriteRenderer.sprite.bounds.size.x + characterSpace) * fontScale;
            }

            for (int i = 0; i < spriteRenderers.Length; ++i) {
                SpriteRenderer spriteRenderer = spriteRenderers[i];
                spriteRenderer.transform.localPosition =
                    new Vector3(characterPositions[i] + characterPosition * 0.5f, 0f, 0f);
            }
        }

        public void UpdateColor() {
            if (spriteRenderers == null) return;

            foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
                spriteRenderer.color = color;
            }
        }
    }
}