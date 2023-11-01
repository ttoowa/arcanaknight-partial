using System.Collections.Generic;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public class UpdateInvoker : MonoBehaviour, ISingletone {
        public static UpdateInvoker Instance { get; private set; }

        private List<IUpdatable> updatableList;

        private void Awake() {
            Instance = this;

            updatableList = new List<IUpdatable>();
        }

        private void Update() {
            IUpdatable[] updatables = updatableList.ToArray();
            foreach (IUpdatable updatable in updatables) {
                if (updatable == null) {
                    Remove(updatable);
                    continue;
                }

                if (!updatable.IsActive)
                    continue;

                updatable.OnTick(Time.deltaTime);
            }
        }

        public void Clear() {
            updatableList.Clear();
        }

        public void Add(IUpdatable updatable, bool startUpdate = true) {
            updatableList.Add(updatable);

            if (startUpdate)
                updatable.IsActive = true;
        }

        public void Remove(IUpdatable updatable) {
            updatableList.Remove(updatable);
        }
    }
}