using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace ArcaneSurvivorsClient.Game {
    public class GamePlayer : IGamePlayer, IDisposable {
        public static GamePlayer LocalPlayer;

        private static GameBalance GameBalance => GameManager.Instance.PlayingGame.PlayingChapter.gameBalance;

        private ObjectPool<DamageFX> damageFxPool = new(() => {
            return FXResource.Instance.playerDamageFXPrefab.InstantiateFX(null, false).Get<DamageFX>();
        });

        public Pawn Pawn { get; private set; }

        public PawnStatusUI StatusUI { get; private set; }

        public readonly PlayableCharacter Character;

        public GamePlayer(PlayableCharacter character) {
            Character = character;

            Pawn = PawnFactory.Spawn(PawnResource.Instance.playerPawnPrefab, new PlayerPawnBrain());
            Pawn.player = this;
            Pawn.Damaged += OnDamaged;
            Pawn.Killed += OnKilled;
            Pawn.Healed += OnHealed;

            Pawn.ability = character.ability;

            StatusUI = Object.Instantiate(GameResource.Instance.playerPawnStatusPrefab, GlobalUI.Instance.canvasTrsf_UI)
                .GetComponent<PawnStatusUI>();
            StatusUI.GetComponent<WorldUI>().target = Pawn.transform;
            StatusUI.transform.SetParent(GlobalUI.Instance.worldUIArea);

            GameResource.Instance.rotationIndicator.target = Pawn.transform;

            damageFxPool.GetInstanceMethod += (value) => {
                value.SetVisible(true);
                //value.SetActive(true);
            };
            damageFxPool.ReturnInstanceMethod += (value) => {
                value.SetVisible(false);
                //value.SetActive(false);
            };
        }

        private void OnHealed(Pawn pawn, float amount) {
            GameManager.Instance.PlayingGame?.Statistics.CollectHealed(amount);
        }

        private void OnDamaged(Pawn pawn, DamageInfo damageInfo) {
            // Count statistics
            GameManager.Instance.PlayingGame?.Statistics.CollectDamageDealt(pawn.force, damageInfo.damage);

            // VFX & SFX
            DamageFX damageFx = damageFxPool.GetInstance();
            damageFx.transform.localPosition = pawn.HeadPosition;
            damageFx.SetDamage(damageInfo.DamageInt);
            damageFx.RestartAnimation();
            JobInvoker.Instance.AddJob(() => {
                damageFxPool.ReturnInstance(damageFx);
            }, 2f);

            SfxPlayer.Play("game.damaged");
        }

        private void OnKilled(Pawn pawn) {
            GameManager.Instance.PlayingGame.GameOver();
        }

        public void Dispose() {
            GameResource.Instance.rotationIndicator.target = null;

            if (Pawn != null) {
                Pawn.Damaged -= OnDamaged;
                Pawn.Killed -= OnKilled;

                Object.Destroy(Pawn.gameObject);
                Object.Destroy(StatusUI.gameObject);
                Pawn = null;
                StatusUI = null;
            }
        }

        public void OnTick(float deltaTime) {
            Vector3 rotation = GameResource.Instance.rotationIndicator.transform.localEulerAngles;
            rotation.y = Pawn.CurrentAngle;
            GameResource.Instance.rotationIndicator.transform.localEulerAngles = rotation;

            StatusUI.SetHP(Pawn.NormalizedHP);
        }

        public void OnTriggerEnter(Collider other) {
            if (other.attachedRigidbody == null) return;
            FieldDropItem fieldDropItem = other.attachedRigidbody.Get<FieldDropItem>();
            if (fieldDropItem == null) return;

            if (other.CompareTag("FieldDropGold")) {
                GameManager.Instance.PlayingGame.AddGold(fieldDropItem.amount);

                GameObject gameObject = fieldDropItem.gameObject;
                if (gameObject != null)
                    gameObject.Destroy();

                FXResource.Instance.pickupFieldItemFXPrefab.InstantiateFX(other.transform.position);
                PickupGoldFX indicatorFX = GameResource.Instance.pickupGoldUIFXPrefab
                    .InstantiateFX(UIUtility.WorldToCanvasPoint(Pawn.HeadPosition + new Vector3(0f, 0f, 1f)), true)
                    .Get<PickupGoldFX>();
                indicatorFX.SetAmount(fieldDropItem.amount);

                SfxPlayer.Play("game.pickup.gold", true);
            } else if (other.CompareTag("FieldDropPotion")) {
                Pawn.HealPercent(GameBalance.potionFieldHealAmountPercent);

                GameObject gameObject = fieldDropItem.gameObject;
                if (gameObject != null)
                    gameObject.Destroy();

                FXResource.Instance.pickupFieldItemFXPrefab.InstantiateFX(other.transform.position);
                SfxPlayer.Play("game.pickup.potion", true);
            }
        }

        public void OnTriggerStay(Collider other) {
        }

        public void OnTriggerExit(Collider other) {
        }

        public void OnCollisionEnter(Collision other) {
        }

        public void OnCollisionStay(Collision other) {
        }

        public void OnCollisionExit(Collision other) {
        }
    }
}