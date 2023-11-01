using System.Collections;
using System.Collections.Generic;
using ArcaneSurvivorsClient;
using ArcaneSurvivorsClient.Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class VPad : MonoBehaviour, ISingletone {
    public static VPad Instance { get; private set; }

    [Range(0f, 0.9f)] public float deadzoneThreshold = 0.3f;

    [SerializeField] private GameObject panel;
    [SerializeField] private RectTransform canvas;
    [SerializeField] private EventTrigger movePadAreaTrigger;
    [SerializeField] private RectTransform movePad;
    [SerializeField] private RectTransform movePadHandle;

    [SerializeField] private Button actionButton;

    // Runtime drag state
    private Vector2 moveInputStartPoint;


    public Vector2 MoveInput { get; private set; }
    public bool OnMoveInput => MoveInput.magnitude > deadzoneThreshold;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        EventTrigger.Entry movePadAreaPointerDown = new() {
            eventID = EventTriggerType.PointerDown
        };
        movePadAreaPointerDown.callback.AddListener((BaseEventData data) => {
            PointerEventData pointerData = data as PointerEventData;
            OnMovePadPointerDown(pointerData.position);
        });
        movePadAreaTrigger.triggers.Add(movePadAreaPointerDown);

        EventTrigger.Entry movePadAreaPointerMove = new() {
            eventID = EventTriggerType.Drag
        };
        movePadAreaPointerMove.callback.AddListener((BaseEventData data) => {
            PointerEventData pointerData = data as PointerEventData;
            OnMovePadPointerMove(pointerData.position);
        });
        movePadAreaTrigger.triggers.Add(movePadAreaPointerMove);

        EventTrigger.Entry movePadAreaPointerUp = new() {
            eventID = EventTriggerType.PointerUp
        };
        movePadAreaPointerUp.callback.AddListener((BaseEventData data) => {
            OnMovePadPointerUp();
        });
        movePadAreaTrigger.triggers.Add(movePadAreaPointerUp);

        OnMovePadPointerUp();

        SetVisible(false);

        GameManager.Instance.GameStarted += OnGameStarted;
        GameManager.Instance.GameEnded += OnGameEnded;
    }

    private void OnGameStarted(IGame game) {
        if (game.RuleType == GameRuleType.Standard) {
            StandardGame standardGame = game as StandardGame;
            standardGame.PhaseChanged += OnPhaseChanged;
        }
    }

    private void OnGameEnded(IGame game) {
        if (game.RuleType == GameRuleType.Standard) {
            StandardGame standardGame = game as StandardGame;
            standardGame.PhaseChanged -= OnPhaseChanged;
        }
    }

    private void OnPhaseChanged(GamePhase gamephase, StandardGame.StageInfo stageInfo) {
        switch (gamephase) {
            default:
            case GamePhase.Day:
                SetVisible(false);
                break;
            case GamePhase.Night:
                SetVisible(true);
                break;
        }
    }

    public void SetVisible(bool visible) {
        if (panel.activeSelf == visible) return;

        panel.SetActive(visible);

        OnMovePadPointerUp();
    }

    private void OnMovePadPointerDown(Vector2 point) {
        Vector2 touchedCanvasPosition = UIUtility.ScreenToCanvasPoint(canvas, point);
        movePad.anchoredPosition = touchedCanvasPosition;
        movePadHandle.anchoredPosition = Vector2.zero;
        moveInputStartPoint = point;
        movePad.gameObject.SetActive(true);
    }

    private void OnMovePadPointerMove(Vector2 point) {
        MoveInput = Vector2.ClampMagnitude(
            UIUtility.ScreenToCanvasPoint(canvas, point - moveInputStartPoint) / (movePad.rect.width * 0.5f), 1f);
        movePadHandle.anchoredPosition = MoveInput * movePad.rect.width * 0.5f;
    }

    private void OnMovePadPointerUp() {
        MoveInput = Vector2.zero;
        movePadHandle.anchoredPosition = Vector2.zero;
        movePad.gameObject.SetActive(false);
    }
}