using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ArcaneSurvivorsClient.Game {
    public class MonsterManager : MonoBehaviour, ISingletone, IPauseable {
        public delegate void MonsterPawnDelegate(MonsterPawn monsterPawn);

        public static MonsterManager Instance { get; private set; }

        public const int MaxMonsterCount = 140;

        private static GameBalance GameBalance => GameManager.Instance.PlayingGame.PlayingChapter.gameBalance;
        public MonsterPawn NearestMonster { get; private set; }
        public int MonsterCount => monsterList.Count;

        public IReadOnlyList<MonsterPawn> MonsterList => monsterList;
        public IReadOnlyList<MonsterPawn> MonsterListByDistance => monsterListByDistance;
        public bool IsSpawning { get; private set; }

        private float spawnCurrentTime;
        private float spawnAccel;
        private List<MonsterPawn> monsterList = new();
        private List<MonsterPawn> monsterListByDistance = new();
        private MonsterSpawnSchedule spawnSchedule;

        private ObjectPool<DamageFX> damageFxPool = new(() => {
            return FXResource.Instance.monsterDamageFXPrefab.InstantiateFX(null, false).Get<DamageFX>();
        });

        private ObjectPool<GameObject> hitFxPool = new(() => {
            return FXResource.Instance.hitFXPrefab.InstantiateFX(null, false);
        });

        public bool RewardActive { get; private set; }
        public bool BehaviorActive { get; private set; }

        public Vector3 NearestMonsterDirection => NearestMonster != null
            ? (NearestMonster.transform.position - GamePlayer.LocalPlayer.Pawn.transform.position).normalized
            : Vector3.zero;

        public event MonsterPawnDelegate Killed;

        private void Awake() {
            Instance = this;

            damageFxPool.GetInstanceMethod += (value) => {
                value.SetVisible(true);
                //value.SetActive(true);
            };
            damageFxPool.ReturnInstanceMethod += (value) => {
                value.SetVisible(false);
                //value.SetActive(false);
            };

            hitFxPool.GetInstanceMethod += (value) => {
                value.SetActive(true);
            };
            hitFxPool.ReturnInstanceMethod += (value) => {
                value.SetActive(false);
            };

            GameManager.Instance.GameEnded += OnGameEnded;
        }

        private void Update() {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.G))
                BehaviorActive = !BehaviorActive;

            if (IsSpawning) {
                DevelopLogger.Log( //
                    $"MonsterCount : {MonsterCount}\n" //
                    + $"SpawnSchedule : {spawnSchedule.CurrentStep} / {spawnSchedule.SpawnStepList.Count}");
            } else
                DevelopLogger.Log("");
#endif

            if (!IsSpawning) return;

            spawnCurrentTime += Time.deltaTime;

            if (MonsterCount < MaxMonsterCount) {
                MonsterSpawnStep? spawnStep = spawnSchedule.GetSpawnStep(spawnCurrentTime);
                if (spawnStep.HasValue)
                    SpawnMonster(spawnStep.Value);

                if (spawnSchedule.IsComplete)
                    spawnCurrentTime = spawnSchedule.GoToMiddle();
            }

            // Manage collections
            NearestMonster = monsterList.FindClosest(x =>
                Vector3.Distance(x.WorldPosition, GamePlayer.LocalPlayer.Pawn.WorldPosition));
        }

        private void LateUpdate() {
            monsterListByDistance = monsterList.Where(x => x.isAlive).OrderBy(x =>
                Vector3.Distance(x.WorldPosition, GamePlayer.LocalPlayer.Pawn.WorldPosition)).ToList();
        }

        private void ClearCollection() {
            monsterList.Clear();
            monsterListByDistance.Clear();
            NearestMonster = null;
        }

        public void ResetTime() {
            // 일정시간 딜레이를 주기 위해 음수로 설정
            spawnCurrentTime = -2f;
        }

        // Monster info
        public MonsterPawn GetRandomMonster() {
            if (monsterList.Count == 0)
                return null;

            return monsterList[Random.Range(0, monsterList.Count)];
        }

        public MonsterPawn GetNearRandomMonster(float maxDistance = 10) {
            if (monsterListByDistance.Count == 0)
                return null;

            int lastIndex = -1;
            for (int i = 0; i < monsterListByDistance.Count; ++i) {
                MonsterPawn monster = monsterListByDistance[i];

                float distance = Vector3.Distance(monster.WorldPosition, GamePlayer.LocalPlayer.Pawn.WorldPosition);
                if (distance > maxDistance) {
                    lastIndex = i - 1;
                    break;
                }
            }

            if (lastIndex >= 0)
                return monsterListByDistance[Random.Range(0, lastIndex + 1)];

            return null;
        }

        // Spawn
        public void SetSpawning(bool spawnState) {
            IsSpawning = spawnState;
        }

        public void SetSpawnSchedule(GameStage gameStage, GameBalance balance, float normalizedDayNum) {
            spawnAccel = balance.monsterSpawnAccel.Sample(normalizedDayNum);
            MonsterSpawnParams spawnParams = new() {
                spawnPool = gameStage.normalMonsterPool,
                maxSpawnSetElementCount = balance.monsterSpawnSetMaxElementCount.SampleRoundInt(normalizedDayNum),
                spawnStep = balance.monsterSpawnStepRange.SampleRoundInt(normalizedDayNum),
                monsterHpSum = balance.monsterSpawnHpSumRange.SampleFloorInt(normalizedDayNum),
                stageDuration = Mathf.Max(0, balance.stageDurationRange.Sample(normalizedDayNum) - 10)
            };
            spawnSchedule = MonsterSpawnLogic.GenerateSpawnSchedule(spawnParams);
        }

        public void SetRewardActive(bool rewardActive) {
            RewardActive = rewardActive;
        }

        public void KillAll() {
            MonsterPawn[] monsters = monsterList.ToArray();
            foreach (MonsterPawn monster in monsters) {
                if (monster == null || !monster.isAlive) continue;

                monster.Kill();
            }

            ClearCollection();
        }

        public void RemoveAll() {
            MonsterPawn[] monsters = monsterList.ToArray();
            foreach (MonsterPawn monster in monsters) {
                if (monster == null || !monster.isAlive) continue;

                monster.Remove();
            }

            ClearCollection();
        }

        // private void OnSpawnTick() {
        //     if (spawnedMonsterList.Count >= MaxMonsterCount)
        //         return;
        // }

        public void SpawnMonster(MonsterSpawnStep spawnStep) {
            foreach (Monster monster in spawnStep.spawnSet.monsters) {
                for (int i = 0; i < monster.spawnScale; ++i) {
                    SpawnMonster(monster.monsterType);
                }
            }
        }

        public void SpawnMonster(MonsterType monsterType) {
            // Vector3 playerPosition = GamePlayer.LocalPlayer.Pawn.transform.position;
            // float angle = Random.Range(0f, 360f);
            // Vector3 spawnPosition = playerPosition + Quaternion.Euler(0f, angle, 0f) * Vector3.forward * SpawnDistance;
            Vector3 spawnPosition = GameManager.Instance.PlayingGame.CurrentStage.map.SampleMonsterSpawnPoint();
            spawnPosition.y = 0f;

            Monster monsterDefine = MonsterResource.Instance.library.GetData(monsterType);
            MonsterPawn monsterPawn =
                PawnFactory.Spawn(monsterDefine.pawnPrefab,
                    MonsterBrainFactory.CreateBrain(monsterDefine.brainType)) as MonsterPawn;
            monsterPawn.transform.SetParent(GameResource.Instance.monsterArea);
            monsterPawn.transform.localPosition = spawnPosition.AddRandomOffset(new Vector3(2f, 0f, 2f));

            monsterList.Add(monsterPawn);

            monsterPawn.Killed += OnMonsterKilled;
            monsterPawn.Removed += OnMonsterRemoved;
            monsterPawn.Damaged += OnMonsterDamaged;

            monsterPawn.ability = monsterDefine.ability.Clone() as PawnAbility;

            monsterPawn.score = (int)monsterDefine.ability.hp;

            monsterPawn.gameObject.InitMonsterHitboxes(monsterPawn, null);

            monsterPawn.ResetState();
        }

        private void OnMonsterDamaged(Pawn pawn, DamageInfo damageInfo) {
            // Count statistics
            GameManager.Instance.PlayingGame?.Statistics.CollectDamageDealt(pawn.force, damageInfo.damage);

            // VFX & SFX
            GameObject hitFxGObj = hitFxPool.GetInstance();
            hitFxGObj.transform.localPosition = pawn.FXPosition;

            JobInvoker.Instance.AddJob(() => {
                hitFxPool.ReturnInstance(hitFxGObj);
            }, 2f);

            // damageFxGObj.GetComponent<RectTransform>().anchoredPosition =
            //     UIUtility.WorldToCanvasPoint(pawn.WorldPosition);
            DamageFX damageFx = damageFxPool.GetInstance();
            damageFx.transform.localPosition = pawn.HeadPosition;
            damageFx.SetDamage(damageInfo.DamageInt);
            damageFx.RestartAnimation();
            JobInvoker.Instance.AddJob(() => {
                damageFxPool.ReturnInstance(damageFx);
            }, 2f);

            SfxPlayer.Play("game.hit", true);
        }

        private void OnMonsterRemoved(Pawn pawn) {
            monsterList.Remove(pawn as MonsterPawn);
        }

        private void OnMonsterKilled(Pawn pawn) {
            MonsterPawn monsterPawn = pawn as MonsterPawn;

            monsterList.Remove(monsterPawn);


            if (RewardActive) {
                float healProbability = StatCalculator.GetHealProbability(PawnForce.Player, 0f);
                if (Random.value < healProbability)
                    GamePlayer.LocalPlayer.Pawn.Heal(5);

                DropGold(pawn);
                DropPotion(pawn);

                // 스폰 타이머 가속
                spawnCurrentTime += spawnAccel;

                GameManager.Instance.PlayingGame?.Statistics.CollectKill();
            }

            Killed?.Invoke(monsterPawn);
        }

        private void DropGold(Pawn pawn) {
            float doubleGoldProbability = StatCalculator.GetDoubleGoldProbability(PawnForce.Player, 0f);
            int count = Random.value < doubleGoldProbability ? 2 : 1;
            for (int i = 0; i < count; ++i) {
                FieldDropItem fieldDropGold = GameResource.Instance.fieldDropGoldPrefab
                    .Instantiate(FXResource.Instance.FXArea, pawn.FXPosition).Get<FieldDropItem>();
                fieldDropGold.amount = 1;
                GameManager.Instance.AddManagedGameObject(fieldDropGold.gameObject);
            }
        }

        private void DropPotion(Pawn pawn) {
            if (Random.value <
                GameBalance.potionFieldDropPercent.Sample(GameManager.Instance.PlayingGame.NormalizedStageNum)) {
                GameObject fieldDropPotion =
                    GameResource.Instance.fieldDropPotionPrefab.Instantiate(FXResource.Instance.FXArea,
                        pawn.FXPosition);
                GameManager.Instance.AddManagedGameObject(fieldDropPotion);
            }
        }

        private void OnGameEnded(IGame game) {
            RemoveAll();
            ClearCollection();
        }
    }
}