using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ArcaneSurvivorsClient {
    public class CanvasForceUpdater : MonoBehaviour {
        private void LateUpdate() {
            LayoutRebuilder.ForceRebuildLayoutImmediate(GlobalUI.Instance.canvasTrsf_UI);
            Canvas.ForceUpdateCanvases();
        }
    }
}