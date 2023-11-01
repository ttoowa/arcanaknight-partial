using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class Weapon_Spear : RuntimeWeaponBase {
        public override void OnEquip() {
        }

        public override void OnDestroy() {
        }

        public override void OnRepeatWeaponTick(float cooltime) {
            switch (OwnerWeaponBundle.level) {
                case 1:
                case 2: {
                    GameObject slash = OwnerWeaponBundle.weapon.prefabResources[0]
                        .InstantiateFX(OwnerPawn.WorldPosition, false);
                    ObjectFollower follower = slash.AddComponent<ObjectFollower>();
                    follower.mode = FollowMode.Immediate;
                    follower.target = OwnerPawn.transform;
                    slash.transform.localEulerAngles = new Vector3(0f, OwnerPawn.CurrentAngle + 90f, 0f);
                    slash.InitPlayerHitboxes(OwnerPawn, this, null);

                    SfxPlayer.Play("weapon.spear.swing", true);
                    break;
                }
                case 3: {
                    GameObject slash = OwnerWeaponBundle.weapon.prefabResources[1]
                        .InstantiateFX(OwnerPawn.WorldPosition, false);
                    ObjectFollower follower = slash.AddComponent<ObjectFollower>();
                    follower.mode = FollowMode.Immediate;
                    follower.target = OwnerPawn.transform;
                    slash.transform.localEulerAngles = new Vector3(0f, OwnerPawn.CurrentAngle + 90f, 0f);
                    slash.InitPlayerHitboxes(OwnerPawn, this, null);

                    SfxPlayer.Play("weapon.spear.swing", true);
                    break;
                }
            }
        }

        public override void OnTick(float deltaTime) {
        }

        public override void OnHit(Pawn pawn) {
            SfxPlayer.Play("weapon.spear.hit", true);
        }

        public override void OnLevelChanged(int level) {
        }
    }
}