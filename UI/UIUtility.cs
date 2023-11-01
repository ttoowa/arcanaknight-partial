using UnityEngine;

namespace ArcaneSurvivorsClient {
    public static class UIUtility {
        public static Vector2 ScreenToCanvasPoint(RectTransform canvas, Vector2 screenPoint) {
            return new Vector2(screenPoint.x / Screen.width * canvas.rect.width,
                screenPoint.y / Screen.height * canvas.rect.height);
        }

        public static Vector2 ScreenToViewportPoint(Vector2 screenPoint) {
            return new Vector2(screenPoint.x / Screen.width, screenPoint.y / Screen.height);
        }

        public static Vector2 WorldToCanvasPoint(Vector3 worldPoint, TransformSpace worldSpace = TransformSpace.World) {
            Camera targetCamera;
            switch (worldSpace) {
                default:
                case TransformSpace.World:
                    targetCamera = Cameras.Instance.camera_World;
                    break;
                case TransformSpace.UI:
                    targetCamera = Cameras.Instance.camera_UI;
                    break;
            }

            RectTransform canvas = GlobalUI.Instance.canvasTrsf_UI;

            Vector3 viewportPosition = targetCamera.WorldToViewportPoint(worldPoint);
            return new Vector2(viewportPosition.x * canvas.sizeDelta.x, viewportPosition.y * canvas.sizeDelta.y);
        }

        public static Vector2 CanvasToWorldPoint(Vector3 canvasPoint, float positionZ = 0f) {
            Camera targetCamera = Cameras.Instance.camera_World;
            RectTransform canvas = GlobalUI.Instance.canvasTrsf_UI;

            Vector2 viewportPosition = new(canvasPoint.x / canvas.sizeDelta.x, canvasPoint.y / canvas.sizeDelta.y);
            Vector3 position = targetCamera.ViewportToWorldPoint(viewportPosition);
            position.z = positionZ;
            return position;
        }

        public static bool IsContainsScreenPoint(this RectTransform element, Vector2 screenPoint) {
            Vector2 canvasPoint = ScreenToCanvasPoint(GlobalUI.Instance.canvasTrsf_UI, screenPoint);
            return IsContainsCanvasPoint(element, canvasPoint);
        }

        public static bool IsContainsCanvasPoint(this RectTransform element, Vector2 canvasPoint) {
            Vector2 elementCanvasPosition = WorldToCanvasPoint(element.transform.position, TransformSpace.UI);

            Vector2 cursorDistance = elementCanvasPosition - canvasPoint;
            cursorDistance.x = Mathf.Abs(cursorDistance.x);
            cursorDistance.y = Mathf.Abs(cursorDistance.y);
            return cursorDistance.x < element.rect.width * 0.5f && cursorDistance.y < element.rect.height * 0.5f;
        }

        public static GameObject[] GetChilds(this Transform trsf) {
            GameObject[] childs = new GameObject[trsf.childCount];
            for (int i = 0; i < trsf.childCount; i++) {
                childs[i] = trsf.GetChild(i).gameObject;
            }

            return childs;
        }

        public static void SetGuageAmount(this RectTransform guageFore, float amount) {
            guageFore.anchorMax = new Vector2(amount, guageFore.anchorMax.y);
        }
    }
}