using System;
using System.Collections;
using ArcaneSurvivorsClient.Analytics;
using ArcaneSurvivorsClient.Game;
using ArcaneSurvivorsClient.Locale;
using Firebase.Analytics;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class StandardGame : MonoBehaviour, IGame {
        public struct StageInfo {
            public GameStage currentStage;
            public GameChapter chapter;
            public StandardGame game;
        }

        public delegate void DayChangedDelegate(StageInfo stage);

        public delegate void PhaseChangedDelegate(GamePhase gamePhase, StageInfo stage);

        public string ContentCode => PlayingChapter.ContentCode;
        public bool IsPlaying => Phase != GamePhase.None;
        public bool IsBattlePhase => Phase == GamePhase.Night && enableInteractive;
        public GameRuleType RuleType => GameRuleType.Standard;


        public float NormalizedStageNum => ((float)StageNum - 1) / (PlayingChapter.stageLibrary.dataObjects.Length - 1);
        public bool IsFirstStage => StageNum == 1;
        public bool IsLastStage => StageNum == PlayingChapter.stageLibrary.dataObjects.Length;
        public GameStage CurrentStage { get; private set; }
        public GameStatistics Statistics { get; private set; }
        public GameChapter PlayingChapter { get; private set; }

        // Game state
        public GamePhase Phase { get; private set; }

        public int StageNum { get; private set; }

        public float Gold { get; private set; }

        public float ScoreProgress { get; private set; }

        public float Score { get; private set; }
        public float MaxScore { get; private set; }

        private GamePlayer localPlayer;

        private int killCount;
        private float collectedGoldAmount;
        private int accRewardSp;

        private bool enableInteractive;
        private bool isPlayingEndNightRoutine;

        public event DayChangedDelegate DayChanged;
        public event PhaseChangedDelegate PhaseChanged;
        public event IGame.ScoreChangedDelegate ScoreProgressChanged;
        public event IGame.GoldAssetChangedDelegate GoldAssetChanged;
        public event IGame.PlayerHpChangedDelegate PlayerHpChanged;

        private void Start() {
            Statistics = new GameStatistics();

            SetPhase(GamePhase.None);

            MonsterManager.Instance.Killed += OnMonsterKilled;
        }

        public void ResetGame() {
            Statistics.Reset();
            SetStage(1);
            SetGold(0);
            SetPhase(GamePhase.Day);

            accRewardSp = 0;
            collectedGoldAmount = 0;
            killCount = 0;
        }

        public void OnTick(float deltaTime) {
        }

        public void StartGame(GameChapter chapter) {
            PlayingChapter = chapter;

            ResetGame();

            WeaponInventory.Instance.Clear();

            WeaponInventory.Instance.AddWeapon(PlayableCharacterManager.Instance.SelectedCharacter.startWeaponType);

            WeaponShopUI.Instance.exitShopButton.onClick.AddListener(OnClickExitShop);

            localPlayer =
                GamePlayerManager.Instance.SpawnLocalPlayer(PlayableCharacterManager.Instance.SelectedCharacter);
            localPlayer.Pawn.hp.ValueChanged += OnPlayerHpChanged;
            localPlayer.Pawn.hp.InvokeValueChanged();
            localPlayer.StatusUI.SetVisible(false);

            SetScore(0);
            SetMaxScore(1);

            // Test starting gold
            if (DevelopSetting.IsDevelopMode)
                SetGold(10000);

            SetPhase(GamePhase.Night);

            ++PlayRecord.Instance.startCount.Value;

            GameAnalytics.LogEvent("GameStarted", // 
                new Parameter("content", ContentCode), // 
                new Parameter("difficulty", PlayingChapter.difficultyState.SelectedDifficulty.level),
                new Parameter("character", PlayableCharacterManager.Instance.SelectedCharacter.name));
        }

        public void EndGame() {
            if (localPlayer != null) {
                localPlayer.Pawn.hp.ValueChanged -= OnPlayerHpChanged;
                GamePlayerManager.Instance.RemoveLocalPlayer();
                localPlayer = null;
            }

            SetPhase(GamePhase.None);

            WeaponInventory.Instance.Clear();
            RuntimeWeaponManager.Instance.ClearWeapons();
            GamePlayerManager.Instance.RemoveLocalPlayer();
            MonsterManager.Instance.SetRewardActive(false);
            MonsterManager.Instance.SetSpawning(false);
            VPad.Instance.SetVisible(false);

            WeaponShopUI.Instance.exitShopButton.onClick.RemoveListener(OnClickExitShop);
        }

        public void GameOver() {
            EndNightPhase(false);
        }

        // Phase & Day
        public void NextStage() {
            AddStage();
            SetPhase(GamePhase.Day);
        }

        public void SetPhase(GamePhase phase, bool force = false) {
            if (Phase == phase && !force) return;

            Phase = phase;

            GameManager.Instance.ClearGameObjects();
            WeaponShopUI.Instance.SetVisible(false);
            CurrentStage.map.RemoveInstance();

            PhaseChanged?.Invoke(phase, new StageInfo() {
                currentStage = CurrentStage,
                chapter = PlayingChapter,
                game = this
            });

            switch (phase) {
                case GamePhase.Day:
                    OnDayPhase();
                    break;
                case GamePhase.Night:
                    OnNightPhase();
                    break;
                case GamePhase.NightResult:
                    OnNightResultPhase();
                    break;
            }
        }

        public void AddStage() {
            SetStage(StageNum + 1);
        }

        public void SetStage(int day) {
            StageNum = day;

            CurrentStage = PlayingChapter.stageLibrary.GetData(day);
            DayChanged?.Invoke(new StageInfo() {
                currentStage = CurrentStage,
                chapter = PlayingChapter,
                game = this
            });
        }

        private void EndNightPhase(bool isTimeOver) {
            if (Phase != GamePhase.Night) return;

            CoroutineDispatcher.Dispatch(EndNightRoutine(isTimeOver));
        }

        private IEnumerator EndNightRoutine(bool isTimeOver) {
            if (isPlayingEndNightRoutine) yield break;
            isPlayingEndNightRoutine = true;

            enableInteractive = false;

            VPad.Instance.SetVisible(false);
            MonsterManager.Instance.SetRewardActive(false);
            MonsterManager.Instance.SetSpawning(false);
            if (isTimeOver) {
                MonsterManager.Instance.KillAll();

                yield return new WaitForSeconds(1f);
            } else
                yield return new WaitForSeconds(3f);

            bool isCleared = localPlayer.Pawn.isAlive;
            bool isLastStage = IsLastStage;
            float fadeOutPreDelay = isCleared && isLastStage && PlayingChapter.endingStoryClip != null ? 1f : 0f;

            ScreenTransition.Show(ScreenTransitionType.AlphaFade, ScreenTransitionType.AlphaFade, () => {
                isPlayingEndNightRoutine = false;

                SetPhase(GamePhase.NightResult);
                localPlayer.StatusUI.SetVisible(false);

                if (isCleared) {
                    // 스테이지 & 액트 클리어
                    if (isLastStage) {
                        PlayingChapter.UnlockNextChapter();

                        // 기록 및 통계 관리
                        ++PlayRecord.Instance.clearCount.Value;
                        ++PlayRecord.Instance.playCount.Value;

                        GameAnalytics.LogEvent("GameCleared", // 
                            new Parameter("content", ContentCode), // 
                            new Parameter("difficulty", PlayingChapter.difficultyState.SelectedDifficulty.level),
                            new Parameter("character", PlayableCharacterManager.Instance.SelectedCharacter.name));

                        GameAnalytics.LogEvent("GameEnded", // 
                            new Parameter("content", ContentCode), // 
                            new Parameter("difficulty", PlayingChapter.difficultyState.SelectedDifficulty.level),
                            new Parameter("character", PlayableCharacterManager.Instance.SelectedCharacter.name),
                            new Parameter("stage", CurrentStage.name), // 
                            new Parameter("rewardSp", accRewardSp));

                        Action showResultAction = new(() => {
                            ScreenTransition.Show(ScreenTransitionType.AlphaFade, ScreenTransitionType.AlphaFade,
                                () => {
                                    StageResultUI.ClearActModel model = new() {
                                        contentName = PlayingChapter.ParentAct.ParentMode.name.ToLocale(),
                                        accRewardSp = accRewardSp,
                                        difficultyLevel = PlayingChapter.difficultyState.SelectedDifficulty.level
                                    };

                                    if (PlayingChapter.difficultyState.SelectedDifficulty.level <
                                        PlayingChapter.difficultyState.MaxDifficultyLevel) {
                                        int unlockLevel = PlayingChapter.difficultyState.SelectedDifficulty.level + 1;
                                        if (PlayingChapter.difficultyState.PlayableDifficultyLevel < unlockLevel) {
                                            PlayingChapter.difficultyState.UnlockPlayableDifficultyLevel(unlockLevel);
                                            model.unlockedDifficultyLevel = unlockLevel;
                                        }
                                    }

                                    StorySceneUI.Instance.SetVisible(false);
                                    StageResultUI.Instance.SetVisible(true);
                                    StageResultUI.Instance.ShowClearAct(model);
                                }, 0.5f);
                        });
                        if (PlayingChapter.endingStoryClip != null) {
                            StorySceneUI.Instance.ScriptEndedOnce += showResultAction;
                            StorySceneUI.Instance.PlayClip(PlayingChapter.endingStoryClip);
                            StorySceneUI.Instance.SetVisible(true);
                        } else
                            showResultAction();
                    } else {
                        // 스테이지 클리어 시
                        StageResultUI.ClearStageModel model = new() {
                            stageNum = StageNum,
                            grade = StageResultGrade.ClearStage,
                            killCount = killCount,
                            collectedGold = (int)collectedGoldAmount,
                            inventoryGold = (int)Gold,
                            difficultyLevel = PlayingChapter.difficultyState.SelectedDifficulty.level
                        };
                        StageResultUI.Instance.SetVisible(true);
                        StageResultUI.Instance.ShowClearStage(model);

                        GameAnalytics.LogEvent("StageCleared", // 
                            new Parameter("content", ContentCode), // 
                            new Parameter("collectedGold", (int)collectedGoldAmount),
                            new Parameter("inventoryGold", (int)Gold),
                            new Parameter("difficulty", PlayingChapter.difficultyState.SelectedDifficulty.level),
                            new Parameter("stage", CurrentStage.name), // 
                            new Parameter("rewardSp", accRewardSp));
                    }

                    // 보상 지급
                    accRewardSp += CurrentStage.reward.sp;
                    GlobalStatus.Instance.AddSP(CurrentStage.reward.sp);
                } else {
                    // 게임 오버
                    StageResultUI.GameOverModel model = new() {
                        stageNum = StageNum,
                        accRewardSp = accRewardSp,
                        difficultyLevel = PlayingChapter.difficultyState.SelectedDifficulty.level
                    };
                    StageResultUI.Instance.SetVisible(true);
                    StageResultUI.Instance.ShowGameOver(model);

                    ++PlayRecord.Instance.gameOverCount.Value;
                    ++PlayRecord.Instance.playCount.Value;

                    GameAnalytics.LogEvent("GameOver", // 
                        new Parameter("content", ContentCode), // 
                        new Parameter("difficulty", PlayingChapter.difficultyState.SelectedDifficulty.level),
                        new Parameter("character", PlayableCharacterManager.Instance.SelectedCharacter.name),
                        new Parameter("stage", CurrentStage.name));

                    GameAnalytics.LogEvent("GameEnded", // 
                        new Parameter("content", ContentCode), // 
                        new Parameter("difficulty", PlayingChapter.difficultyState.SelectedDifficulty.level),
                        new Parameter("character", PlayableCharacterManager.Instance.SelectedCharacter.name),
                        new Parameter("stage", CurrentStage.name), new Parameter("rewardSp", accRewardSp));
                }

                MonsterManager.Instance.RemoveAll();
            }, fadeOutPreDelay);
        }

        private void OnClickExitShop() {
            if (Phase == GamePhase.Day) {
                StageChangeDirectingUI.Show(CurrentStage.map.name, CurrentStage.Name, () => {
                    SetPhase(GamePhase.Night);
                });
            }
        }

        private void OnDayPhase() {
            BgmPlayer.Play("game.shopPhase.00");

            WeaponShop.Instance.RerollWeaponGoods(true);
            WeaponShop.Instance.RefillPotionGoods();

            WeaponShopUI.Instance.SetVisible(true);
        }

        private void OnNightPhase() {
            BgmPlayer.Play("game.battlePhase.00");

            enableInteractive = true;

            CurrentStage.map.CreateInstance();

            Vector3 spawnPoint = CurrentStage.map.SamplePlayerSpawnPoint();
            spawnPoint.y = 0f;
            GamePlayer.LocalPlayer.Pawn.WorldPosition = spawnPoint;

            VPad.Instance.SetVisible(true);
            localPlayer.StatusUI.SetVisible(true);

            SetScore(0);
            killCount = 0;
            collectedGoldAmount = 0f;

            MonsterManager.Instance.ResetTime();
            MonsterManager.Instance.SetRewardActive(true);
            MonsterManager.Instance.SetSpawning(true);
            MonsterManager.Instance.SetSpawnSchedule(CurrentStage, PlayingChapter.gameBalance, NormalizedStageNum);

            GameResource.Instance.nightPhaseUI.gameTimer.TimeOutOnce += () => {
                EndNightPhase(true);
            };
            GameResource.Instance.nightPhaseUI.gameTimer.SetLeftSeconds(
                PlayingChapter.gameBalance.stageDurationRange.Sample(NormalizedStageNum));
            GameResource.Instance.nightPhaseUI.gameTimer.SetRunning(true);

            RuntimeWeaponManager.Instance.CreateInventoryWeapons();
        }

        private void OnNightResultPhase() {
            MonsterManager.Instance.SetSpawning(false);
            RuntimeWeaponManager.Instance.ClearWeapons();

            // if (localPlayer != null) {
            //     localPlayer.Pawn.hp.ValueChanged -= OnPlayerHpChanged;
            //     GamePlayerManager.Instance.RemoveLocalPlayer();
            //     localPlayer = null;
            // }
        }

        private void OnPlayerHpChanged(float hp) {
            PlayerHpChanged?.Invoke(hp, localPlayer.Pawn.ActualMaxHp);
        }

        public void SkipNightPhase() {
            GameResource.Instance.nightPhaseUI.gameTimer.SetLeftSeconds(0.5f);
        }

        // Asset
        public void SetGold(float amount) {
            float oldAmount = Gold;
            Gold = amount;

            GoldAssetChanged?.Invoke(oldAmount, amount);

            // Collect statistics
            float delta = amount - oldAmount;
            if (delta > 0)
                Statistics.CollectGoldCollected(delta);
        }

        public void AddGold(float amount) {
            if (amount < 0f) {
                LogBuilder.BuildException(LogType.Error, nameof(StandardGame),
                    "Amount is less than 0. Use SubtractGold instead.", new LogElement("Gold", Gold.ToString("0")),
                    new LogElement("Amount", amount.ToString("0")));
            }

            collectedGoldAmount += (float)amount;
            SetGold(Gold + amount);
        }

        public bool SubtractGold(float amount) {
            if (amount < 0f) {
                LogBuilder.BuildException(LogType.Error, nameof(StandardGame),
                    "Amount is less than 0. Use AddGold instead.", new LogElement("Gold", Gold.ToString("0")),
                    new LogElement("Amount", amount.ToString("0")));
            }

            if (Gold < amount) return false;
            SetGold(Gold - amount);
            return true;
        }

        // Progress
        public void SetMaxScore(float score) {
            MaxScore = score;

            OnScoreChanged();
        }

        public void AddScore(float score) {
            SetScore(Score + score);

            OnScoreChanged();
        }

        public void SetScore(float score) {
            Score = score;

            OnScoreChanged();
        }

        private void OnScoreChanged() {
            if (MaxScore > 0)
                ScoreProgress = Score / MaxScore;
            else
                ScoreProgress = 0;

            ScoreProgressChanged?.Invoke(new IGame.ScoreChangedInfo() {
                score = Score,
                maxScore = MaxScore,
                progress = ScoreProgress
            });
        }

        private void SetScoreProgress(float progress) {
        }

        private void OnMonsterKilled(MonsterPawn monsterPawn) {
            if (MonsterManager.Instance.RewardActive) {
                ++killCount;

                AddScore(monsterPawn.score);
            }
        }
    }
}