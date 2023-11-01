using System;
using ArcaneSurvivorsClient.Locale;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient.Game {
    public class StorySceneUI : MonoBehaviour {
        public static StorySceneUI Instance { get; private set; }

        private const float CharRenderSpeed = 0.03f;
        private const float CursorBlinkSpeed = 0.5f;

        public StoryClip PlayingClip { get; private set; }

        public GameObject panel;

        public Image illustImage;
        public TextMeshProUGUI scriptContentText;
        public GameObject cursor;
        public Button skipButton;


        // 일단 스크립트 플레이어 기능을 여기에 구현하고,
        // 나중에 기능이 복잡해지면 클래스를 분리할 예정
        private string renderText;
        private string remainedText;
        private int framePosition;

        private bool isPaused;

        private readonly SimpleTimer charRenderTimer = new(CharRenderSpeed);
        private readonly SimpleTimer cursorBlinkTimer = new(CursorBlinkSpeed);

        public event Action ScriptStarted;
        public event Action ScriptEnded;
        public event Action ScriptEndedOnce;

        private void Awake() {
            Instance = this;

            SetVisible(false);
        }

        private void Start() {
            skipButton.onClick.AddListener(() => {
                EndScript();
            });
        }

        private void Update() {
            if (PlayingClip == null) return;

            if (Input.GetMouseButtonDown(0)) {
                if (isPaused)
                    isPaused = false;
                else {
                    if (remainedText.Length == 0) {
                        NextFrame();
                        return;
                    }

                    RenderChar();
                }
            }

            float deltaTime = Time.deltaTime;
            if (!isPaused && charRenderTimer.Tick(deltaTime))
                RenderChar();

            if (cursorBlinkTimer.Tick(deltaTime))
                FlipCursor();
        }

        public void SetVisible(bool visible) {
            panel.SetActive(visible);
        }

        public void PlayClip(StoryClip clip) {
            PlayingClip = clip;

            framePosition = -1;
            NextFrame();

            ScriptStarted?.Invoke();
        }

        public void EndScript() {
            PlayingClip = null;
            framePosition = -1;

            ScriptEnded?.Invoke();
            ScriptEndedOnce?.Invoke();
            ScriptEndedOnce = null;
        }

        public void NextFrame() {
            ++framePosition;
            if (framePosition >= PlayingClip.frames.Length) {
                EndScript();
                return;
            }

            StoryFrame frame = PlayingClip.frames[framePosition];
            if (frame.illustSprite != null)
                illustImage.sprite = frame.illustSprite;

            renderText = "";
            remainedText = frame.scriptContent.ToLocale();
        }

        private void RenderChar() {
            if (remainedText.Length == 0) return;

            if (remainedText.StartsWith("<p>")) {
                remainedText = remainedText.Substring("<p>".Length);
                isPaused = true;
            }

            renderText += remainedText[0];
            remainedText = remainedText.Substring(1);

            scriptContentText.text = renderText;
        }

        private void FlipCursor() {
            if (remainedText.Length == 0 || isPaused)
                cursor.SetActive(!cursor.activeSelf);
            else
                cursor.SetActive(false);
        }
    }
}