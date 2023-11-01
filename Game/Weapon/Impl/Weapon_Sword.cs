using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class Weapon_Sword : RuntimeWeaponBase {
        public override void OnEquip() {
        }

        public override void OnDestroy() {
        }

        public override void OnRepeatWeaponTick(float cooltime) {
            switch (OwnerWeaponBundle.level) {
                case 1:
                case 2: {
                    GameObject slash = OwnerWeaponBundle.weapon.prefabResources[OwnerWeaponBundle.level - 1]
                        .InstantiateFX(OwnerPawn.WorldPosition, false);
                    ObjectFollower follower = slash.AddComponent<ObjectFollower>();
                    follower.mode = FollowMode.Immediate;
                    follower.target = OwnerPawn.transform;
                    slash.transform.localEulerAngles = new Vector3(0f, OwnerPawn.CurrentAngle, 0f);
                    slash.InitPlayerHitboxes(OwnerPawn, this, null);

                    SfxPlayer.Play("weapon.sword.swing", true);
                    break;
                }
                case 3: {
                    GameObject slash = OwnerWeaponBundle.weapon.prefabResources[2]
                        .InstantiateFX(OwnerPawn.FXPosition, false);
                    slash.transform.localEulerAngles = new Vector3(0f, OwnerPawn.CurrentAngle, 0f);
                    slash.InitPlayerHitboxes(OwnerPawn, this, null);
                    slash.GetComponent<ThrowingObject>().Normal = OwnerPawn.CurrentAngle.ToNormal();
                    break;
                }
            }
        }

        public override void OnTick(float deltaTime) {
        }

        public override void OnHit(Pawn pawn) {
            SfxPlayer.Play("weapon.sword.hit", true);
        }

        public override void OnLevelChanged(int level) {
        }
    }
}