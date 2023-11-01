using System;
using System.Collections.Generic;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class GameModeSelector : MonoBehaviour {
        public delegate void GameModeSelectedDelegate(GameMode mode);

        public delegate void GameActSelectedDelegate(GameAct act);

        public delegate void GameChapterSelectedDelegate(GameChapter chapter);

        public static GameModeSelector Instance { get; private set; }

        public GameMode SelectedMode { get; private set; }
        public GameAct SelectedAct { get; private set; }
        public GameChapter SelectedChapter { get; private set; }

        public event GameModeSelectedDelegate OnGameModeSelected;
        public event GameActSelectedDelegate OnGameActSelected;
        public event GameChapterSelectedDelegate OnGameChapterSelected;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
        }

        public void SelectMode(GameMode mode) {
            SelectedMode = mode;

            OnGameModeSelected?.Invoke(mode);
        }

        public void SelectAct(GameAct act) {
            SelectedAct = act;

            OnGameActSelected?.Invoke(act);
        }

        public void SelectChapter(GameChapter chapter) {
            SelectedChapter = chapter;

            OnGameChapterSelected?.Invoke(chapter);
        }
    }
}