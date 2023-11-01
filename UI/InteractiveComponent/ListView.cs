using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient {
    public class ListView : MonoBehaviour {
        public delegate void ElementSelectedStateChangedDelegate(ISelectableElement element, bool isSelected);

        public GameObject itemPrefab;

        public readonly List<ISelectableElement> itemList = new();

        public bool addChildItems = false;

        public event ElementSelectedStateChangedDelegate ElementSelectedStateChanged;

#if UNITY_EDITOR
        private DateTime lastErrorTime;

        private void OnDrawGizmos() {
            if (itemPrefab != null) {
                if (itemPrefab.Get<ISelectableElement>() == null)
                    LogError("itemPrefab must have ISelectableElement component");
            }
        }

        private void LogError(string message) {
            if ((DateTime.Now - lastErrorTime).TotalSeconds > 1) {
                lastErrorTime = DateTime.Now;
                Debug.LogError(message);
            }
        }
#endif

        private void Awake() {
            if (addChildItems) {
                int index = 0;
                ISelectableElement[] elements = GetComponentsInChildren<ISelectableElement>();
                foreach (ISelectableElement element in elements) {
                    element.Id = index.ToString();
                    AddItem(element);
                    ++index;
                }
            }
        }

        public void ClearItems() {
            gameObject.ClearChilds();
            itemList.Clear();
        }

        public ISelectableElement AddItem(string id) {
            ISelectableElement item = itemPrefab.Instantiate(transform).Get<ISelectableElement>();
            item.Id = id;

            return AddItem(item);
        }

        public ISelectableElement AddItem(ISelectableElement item) {
            itemList.Add(item);

            (item as MonoBehaviour).Get<Button>().onClick.AddListener(() => {
                SelectItem(item);
            });
            item.SelectedStateChanged += (bool isSelected) => {
                OnElementSelectedStateChanged(item, isSelected);
            };

            return item;
        }

        public void RemoveItem(ISelectableElement item) {
            itemList.Remove(item);
            Destroy((item as MonoBehaviour).gameObject);
        }

        public void SelectItem(string id, bool withEvent = true) {
            foreach (ISelectableElement element in itemList) {
                if (element.Id == null) continue;
                if (element.Id.ToLower() == id.ToLower()) {
                    SelectItem(element, withEvent);
                    return;
                }
            }
        }

        public void SelectItem(ISelectableElement item, bool withEvent = true) {
            foreach (ISelectableElement element in itemList) {
                if (element == item)
                    element.SetSelected(true, withEvent);
                else
                    element.SetSelected(false, withEvent);
            }
        }

        public void SelectFirstItem(bool withEvent = true) {
            if (itemList.Count > 0)
                itemList[0].SetSelected(true, withEvent);
        }

        public void OnElementSelectedStateChanged(ISelectableElement item, bool isSelected) {
            if (isSelected) {
                foreach (ISelectableElement element in itemList) {
                    element.SetSelected(element == item, false);
                }
            }

            ElementSelectedStateChanged?.Invoke(item, isSelected);
        }
    }
}