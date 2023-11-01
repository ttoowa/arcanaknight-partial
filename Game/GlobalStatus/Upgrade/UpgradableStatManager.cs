using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class UpgradableStatManager : MonoBehaviour {
        public static UpgradableStatManager Instance { get; private set; }

        public UpgradableStatBundle[] StatBundles;

        private readonly Dictionary<UpgradableStatType, UpgradableStatBundle> statBundleDict = new();

        private int usedSp;

        private void Awake() {
            Instance = this;
        }

        private void Start() {
            CreateBundles();
        }

        public JObject ToJObject() {
            JObject jUpgradableStats = new();

            for (int i = 0; i < StatBundles.Length; ++i) {
                UpgradableStatBundle bundle = StatBundles[i];
                jUpgradableStats[bundle.Define.type.ToString()] = bundle.ToJObject();
            }

            jUpgradableStats["usedSP"] = usedSp;

            return jUpgradableStats;
        }

        public void LoadFromJObject(JObject jUpgradableStats) {
            if (jUpgradableStats == null) return;

            for (int i = 0; i < StatBundles.Length; ++i) {
                UpgradableStatBundle bundle = StatBundles[i];
                bundle.LoadFromJObject(jUpgradableStats[bundle.Define.type.ToString()] as JObject);
            }

            usedSp = jUpgradableStats.Value<int>("usedSP");
        }

        private void CreateBundles() {
            StatBundles = new UpgradableStatBundle[UpgradableStatResource.Instance.library.dataObjects.Length];
            UpgradableStat[] statDefines = UpgradableStatResource.Instance.library.dataObjects;
            for (int i = 0; i < statDefines.Length; ++i) {
                UpgradableStat statDefine = statDefines[i];
                UpgradableStatBundle bundle = new(statDefine);
                StatBundles[i] = bundle;
                statBundleDict.Add(statDefine.type, bundle);
            }
        }

        public UpgradableStatBundle GetStatBundle(UpgradableStatType type) {
            return statBundleDict[type];
        }

        public static StatValue GetStatValue(UpgradableStatType type) {
            return Instance.statBundleDict[type].CurrentValue;
        }

        public void ResetLevels() {
            GlobalStatus.Instance.AddSP(usedSp);

            usedSp = 0;
            for (int i = 0; i < StatBundles.Length; ++i) {
                StatBundles[i].SetLevel(0);
            }

            SaveData.MarkAsDirty();
        }

        public void AddUsedSp(int sp) {
            usedSp += sp;

            SaveData.MarkAsDirty();
        }
    }
}