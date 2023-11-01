using System;
using System.Linq;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    public class SynergyBundle {
        public string DisplayLevel => $"{Level}/{synergy.MaxLevel}";
        public Synergy synergy;
        public int Level { get; private set; }

        public StatValue[] ActualBuffValues {
            get {
                SynergyBuffSpec[] buffSpecs = synergy.buffSpecs.OrderBy(x => x.conditionLevel).ToArray();
                for (int buffI = buffSpecs.Length - 1; buffI >= 0; --buffI) {
                    if (Level >= buffSpecs[buffI].conditionLevel)
                        return buffSpecs[buffI].buffValues;
                }

                return null;
            }
        }

        public int ActualBuffLevel {
            get {
                SynergyBuffSpec[] buffSpecs = synergy.buffSpecs.OrderBy(x => x.conditionLevel).ToArray();
                for (int buffI = buffSpecs.Length - 1; buffI >= 0; --buffI) {
                    if (Level >= buffSpecs[buffI].conditionLevel)
                        return buffI + 1;
                }

                return 0;
            }
        }

        public Action LevelChanged;

        public SynergyBundle(Synergy synergy) {
            this.synergy = synergy;
        }

        public SynergyBundle(SynergyType synergyType) : this(SynergyResource.Instance.library.GetData(synergyType)) {
        }

        public void SetLevel(int level) {
            Level = Mathf.Clamp(level, 1, synergy.MaxLevel);

            LevelChanged?.Invoke();
        }

        public void AddLevel(int amount) {
            SetLevel(Level + amount);
        }
    }
}