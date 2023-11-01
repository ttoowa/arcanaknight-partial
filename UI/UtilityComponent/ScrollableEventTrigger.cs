using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient {
    public class ScrollableEventTrigger : EventTrigger {
        public ScrollRect scrollRect;

        private void Awake() {
            scrollRect = GetComponentInParent<ScrollRect>();
        }

        public override void OnBeginDrag(PointerEventData eventData) {
            base.OnBeginDrag(eventData);
            scrollRect.OnBeginDrag(eventData);
        }

        public override void OnDrag(PointerEventData eventData) {
            base.OnDrag(eventData);
            scrollRect.OnDrag(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData) {
            base.OnEndDrag(eventData);
            scrollRect.OnEndDrag(eventData);
        }
    }
}