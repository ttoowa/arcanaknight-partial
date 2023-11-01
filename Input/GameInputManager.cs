using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using ArcaneSurvivorsClient.Game;

namespace ArcaneSurvivorsClient {
    public class GameInputManager : MonoBehaviour, ISingletone, IPauseable {
        public static GameInputManager Instance { get; private set; }

        public Pawn targetPlayerPawn;

        public IGameInput[] gameInputs;

        public GameInput<Vector2> input_Move;

        public ButtonEvent attackButton;
        public GameInput input_Action;

        private void Awake() {
            Instance = this;

            // Move
            input_Move = new GameInput<Vector2>(GameInputRate.TriggerHold);
            input_Move.action += (Vector2 direction) => {
                if (targetPlayerPawn == null)
                    return;

                targetPlayerPawn.Move(direction);
            };
            input_Move.triggerList.Add(new GameInputTrigger<Vector2>(null, () => {
                Vector2 moveInput = Vector2.zero;
                if (Input.GetKey(KeyCode.A))
                    moveInput += Vector2.left;

                if (Input.GetKey(KeyCode.D))
                    moveInput += Vector2.right;

                if (Input.GetKey(KeyCode.W))
                    moveInput += Vector2.up;

                if (Input.GetKey(KeyCode.S))
                    moveInput += Vector2.down;

                moveInput = moveInput.normalized;
                return new Tuple<bool, Vector2>(moveInput.magnitude > 0.1f, moveInput);
            }));
            input_Move.triggerList.Add(new GameInputTrigger<Vector2>(null, () => {
                VPad vPad = VPad.Instance;
                return new Tuple<bool, Vector2>(vPad.OnMoveInput, vPad.MoveInput);
            }));

            // Action
            input_Action = new GameInput(GameInputRate.TriggerEnter);
            input_Action.action += () => {
                if (targetPlayerPawn == null)
                    return;

                targetPlayerPawn.Attack();
            };

            input_Action.triggerList.Add(new GameInputTrigger(null, () => {
                return attackButton.IsClicked;
            }));
            input_Action.triggerList.Add(new GameInputTrigger(KeyCode.Q));

            gameInputs = new IGameInput[] {
                input_Move,
                input_Action
            };
        }

        private void Update() {
            foreach (IGameInput gameInput in gameInputs) {
                gameInput.UpdateAndRun();
            }
        }
    }
}