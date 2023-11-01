using System.Collections.Generic;

namespace ArcaneSurvivorsClient.Game {
    public class AbnormalSet {
        private readonly List<Abnormal> abnormalList = new();
        private readonly Dictionary<string, Abnormal> abnormalDict = new();

        public readonly Pawn OwnerPawn;

        public AbnormalSet(Pawn pawn) {
            OwnerPawn = pawn;
        }

        public void Clear() {
            foreach (Abnormal abnormal in abnormalList) {
                abnormal.OnDisabled();
            }

            abnormalList.Clear();
            abnormalDict.Clear();
        }

        public void Clear(AbnormalEffect effect) {
            foreach (Abnormal abnormal in abnormalList) {
                if (abnormal.effect == effect) {
                    abnormal.OnDisabled();

                    abnormalDict.Remove(abnormal.guid);
                }
            }

            abnormalList.RemoveAll(abnormal => abnormal.effect == effect);
        }

        public void OnTick(float deltaTime) {
            List<Abnormal> removeList = new();
            foreach (Abnormal abnormal in abnormalList) {
                abnormal.OnTick(deltaTime);

                if (abnormal.leftTime <= 0f)
                    removeList.Add(abnormal);
            }

            foreach (Abnormal abnormal in removeList) {
                RemoveAbnormal(abnormal);
            }
        }

        public void AddAbnormal(Abnormal abnormal) {
            // 이미 동일한 guid의 상태이상이 존재하면 leftTime만 갱신
            if (abnormalDict.ContainsKey(abnormal.guid)) {
                abnormalDict[abnormal.guid].leftTime = abnormal.leftTime;
                return;
            }

            abnormalList.Add(abnormal);
            if (!string.IsNullOrWhiteSpace(abnormal.guid))
                abnormalDict.Add(abnormal.guid, abnormal);

            abnormal.OnEnabled();

            OnAbnormalChanged();
        }

        public void RemoveAbnormal(Abnormal abnormal) {
            abnormalList.Remove(abnormal);
            abnormalDict.Remove(abnormal.guid);
            abnormal.OnDisabled();

            OnAbnormalChanged();
        }

        public bool HasAbnormal(AbnormalType type) {
            foreach (Abnormal abnormal in abnormalList) {
                if (abnormal.type == type)
                    return true;
            }

            return false;
        }

        public bool HasAbnormal(string guid) {
            return abnormalDict.ContainsKey(guid);
        }

        public Abnormal GetAbnormal(AbnormalType type) {
            foreach (Abnormal abnormal in abnormalList) {
                if (abnormal.type == type)
                    return abnormal;
            }

            return null;
        }

        public Abnormal GetAbnormal(string guid) {
            if (abnormalDict.ContainsKey(guid))
                return abnormalDict[guid];

            return null;
        }

        private void OnAbnormalChanged() {
            OwnerPawn.SetMaterialBool("_IsPoisoned", HasAbnormal(AbnormalType.Poison));
        }
    }
}