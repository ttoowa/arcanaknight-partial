using UnityEngine;

namespace ArcaneSurvivorsClient {
    public interface ISelectableElement {
        public delegate void SelectedStateChangedDelegate(bool isSelected);

        public string Id { get; set; }

        public event SelectedStateChangedDelegate SelectedStateChanged;
        public void SetSelected(bool selected, bool withEvent = true);
    }
}