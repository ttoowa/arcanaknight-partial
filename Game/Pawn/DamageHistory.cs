using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class DamageHistory {
        private Dictionary<string, DamageInfo> damageInfoDict;

        public DamageHistory() {
            damageInfoDict = new Dictionary<string, DamageInfo>();
        }

        public void Clear() {
            damageInfoDict.Clear();
        }

        public bool Add(DamageInfo damageInfo) {
            if (string.IsNullOrWhiteSpace(damageInfo.hitboxId))
                return true;

            if (damageInfoDict.ContainsKey(damageInfo.hitboxId))
                return false;

            if (damageInfo.duration > 0f)
                damageInfoDict.Add(damageInfo.hitboxId, damageInfo);

            return true;
        }

        public void OnTick(float deltaTime) {
            DamageInfo[] damageInfos = damageInfoDict.Values.ToArray();
            foreach (DamageInfo damageInfo in damageInfos) {
                if (damageInfo.IsExpired)
                    damageInfoDict.Remove(damageInfo.hitboxId);
            }
        }
    }
}