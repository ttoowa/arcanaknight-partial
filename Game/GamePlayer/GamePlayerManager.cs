using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ArcaneSurvivorsClient.Game {
    public class GamePlayerManager : MonoBehaviour, ISingletone {
        public static GamePlayerManager Instance { get; private set; }

        public GamePlayer LocalPlayer { get; private set; }

        public Transform spawnLocator;
        public GameObject dummyPawnPrefab;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
        }

        private void Update() {
            if (LocalPlayer != null)
                LocalPlayer.OnTick(Time.deltaTime);
        }

        public GamePlayer SpawnLocalPlayer(PlayableCharacter character) {
            LocalPlayer = new GamePlayer(character);
            GamePlayer.LocalPlayer = LocalPlayer;

            LocalPlayer.Pawn.transform.SetParent(GameResource.Instance.playerArea);
            LocalPlayer.Pawn.transform.localPosition = spawnLocator.position;

            Cameras.Instance.gimball_World.GetComponent<ObjectFollower>().target = LocalPlayer.Pawn.transform;
            GameInputManager.Instance.targetPlayerPawn = LocalPlayer.Pawn;

            return LocalPlayer;
        }

        public void RemoveLocalPlayer() {
            if (LocalPlayer == null) return;

            LocalPlayer.Dispose();
            LocalPlayer = null;
            GamePlayer.LocalPlayer = null;
        }
    }
}