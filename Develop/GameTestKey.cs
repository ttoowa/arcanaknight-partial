using System.Collections;
using ArcaneSurvivorsClient;
using ArcaneSurvivorsClient.Game;
using ArcaneSurvivorsClient.Locale;
using UnityEngine;

namespace Script.Debugging {
    public class GameTestKey : MonoBehaviour {
#if UNITY_EDITOR
        private void Update() {
            if (Input.GetKeyDown(KeyCode.F1))
                LocaleManager.Instance.SetLanguage("Korean");

            if (Input.GetKeyDown(KeyCode.F2))
                LocaleManager.Instance.SetLanguage("English");

            if (Input.GetKeyDown(KeyCode.F3))
                LocaleManager.Instance.SetLanguage("Thai");

            if (Input.GetKeyDown(KeyCode.F4))
                LocaleManager.Instance.SetLanguage("Japanese");

            if (Input.GetKeyDown(KeyCode.F5))
                LocaleManager.Instance.SetLanguage("Indonesian");

            if (Input.GetKeyDown(KeyCode.F6))
                LocaleManager.Instance.SetLanguage("Vietnamese Language");

            if (Input.GetKeyDown(KeyCode.F7))
                LocaleManager.Instance.SetLanguage("French");

            if (Input.GetKeyDown(KeyCode.F8))
                LocaleManager.Instance.SetLanguage("Simplified Chinese");

            if (Input.GetKeyDown(KeyCode.F9))
                LocaleManager.Instance.SetLanguage("Traditional Chinese");

            if (Input.GetKeyDown(KeyCode.Alpha1))
                WeaponInventory.Instance.AddWeapon(WeaponType.LongSword);

            if (Input.GetKeyDown(KeyCode.Alpha2))
                WeaponInventory.Instance.AddWeapon(WeaponType.Spear);

            if (Input.GetKeyDown(KeyCode.Alpha3))
                WeaponInventory.Instance.AddWeapon(WeaponType.SeekerMine);

            if (Input.GetKeyDown(KeyCode.Alpha4))
                WeaponInventory.Instance.AddWeapon(WeaponType.PoisonCloud);

            if (Input.GetKeyDown(KeyCode.Alpha5))
                WeaponInventory.Instance.AddWeapon(WeaponType.Dagger);


            if (Input.GetKeyDown(KeyCode.Plus))
                GameManager.Instance.PlayingGame.AddGold(1000);

            if (Input.GetKeyDown(KeyCode.P)) {
                // PauseUI.Instance.SetVisible(true);
                if (PauseManager.Instance.IsPaused)
                    PauseManager.Instance.Resume();
                else
                    PauseManager.Instance.Pause();
            }

            if (Input.GetKeyDown(KeyCode.H))
                GamePlayer.LocalPlayer?.Pawn.HealFull();

            if (Input.GetKeyDown(KeyCode.K))
                GamePlayer.LocalPlayer?.Pawn.Kill();

            if (Input.GetKeyDown(KeyCode.Escape))
                (GameManager.Instance.PlayingGame as StandardGame)?.SkipNightPhase();
        }
#endif
    }
}