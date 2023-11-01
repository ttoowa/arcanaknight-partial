using System;
using System.Collections;
using System.Collections.Generic;
using ArcaneSurvivorsClient;
using ArcaneSurvivorsClient.Game;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WorldUI : MonoBehaviour, IUpdatable {
    public bool IsActive { get; set; }

    public Transform target;
    public Vector3 offset;

    public bool useVisibleDistance;
    public float visibleDistance;
    public Func<bool> visibleCondition;

    private RectTransform rectTrsf;

    private void Start() {
        rectTrsf = GetComponent<RectTransform>();
        rectTrsf.anchorMin = Vector2.zero;
        rectTrsf.anchorMax = Vector2.zero;

        UpdateInvoker.Instance.Add(this);
    }

    public void OnTick(float deltaTime) {
        UpdatePosition();
    }

    private void UpdatePosition() {
        if (target == null || !IsActive) return;

        Vector3 targetPosition = target.position + offset;

        // 캐릭터와의 거리에 따라 출력여부 결정
        if (useVisibleDistance) {
            if (GamePlayer.LocalPlayer == null) {
                gameObject.SetActive(false);
                return;
            }

            Vector3 playerPosition = GamePlayer.LocalPlayer.Pawn.transform.position;
            float distanceFromPlayer = Vector3.Distance(playerPosition, targetPosition);

            if (distanceFromPlayer > visibleDistance || (visibleCondition != null && !visibleCondition())) {
                gameObject.SetActive(false);
                return;
            }
        }

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        // UI 좌표 계산
        rectTrsf.anchoredPosition = UIUtility.WorldToCanvasPoint(targetPosition);
    }
}