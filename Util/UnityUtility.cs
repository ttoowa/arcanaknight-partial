using System;
using ArcaneSurvivorsClient.Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace ArcaneSurvivorsClient {
    public static class UnityUtility {
        public static void ClearChilds(this GameObject gameObject) {
            ClearChilds(gameObject.transform);
        }

        public static void ClearChilds(this Transform transform) {
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; --i) {
#if UNITY_EDITOR
                if (Application.isPlaying)
                    Object.Destroy(transform.GetChild(i).gameObject);
                else
                    Object.DestroyImmediate(transform.GetChild(i).gameObject);
#else
                Object.Destroy(transform.GetChild(i).gameObject);
#endif
            }
        }

        public static GameObject[] GetChilds(Transform transform) {
            int childCount = transform.childCount;
            GameObject[] childs = new GameObject[childCount];
            for (int i = 0; i < childCount; ++i) {
                childs[i] = transform.GetChild(i).gameObject;
            }

            return childs;
        }

        public static void ChangeLayersRecursively(Transform transform, int layer) {
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; ++i) {
                ChangeLayersRecursively(transform.GetChild(i), layer);
            }
        }

        public static GameObject Instantiate(this GameObject prefab, Transform parent = null,
            Vector3? localPosition = null, float lifetime = -1f) {
            GameObject instance = Object.Instantiate(prefab, parent);
            if (localPosition.HasValue)
                instance.transform.localPosition = localPosition.Value;
            if (lifetime >= 0f)
                Object.Destroy(instance, lifetime);

            return instance;
        }

        public static GameObject InstantiateFX(this GameObject prefab, Vector3? localPosition = null, bool isUI = false,
            float lifetime = -1f, bool removeScripts = false) {
            GameObject instance =
                Object.Instantiate(prefab, isUI ? GlobalUI.Instance.worldFXArea : FXResource.Instance.FXArea);

            if (localPosition.HasValue) {
                if (isUI)
                    instance.GetComponent<RectTransform>().anchoredPosition = localPosition.Value;
                else
                    instance.transform.localPosition = localPosition.Value;
            }

            if (removeScripts) {
                foreach (MonoBehaviour script in instance.GetComponentsInChildren<MonoBehaviour>()) {
                    if (script.GetType().IsSubclassOf(typeof(Renderer)) || script is Graphic ||
                        script is UIBehaviour) continue;

                    Object.Destroy(script);
                }
            }

            if (lifetime >= 0f)
                Object.Destroy(instance, lifetime);

            return instance;
        }

        public static void Destroy(this GameObject gameObject, float delay = 0f) {
            if (delay > 0f)
                Object.Destroy(gameObject, delay);
            else
                Object.Destroy(gameObject);
        }

        public static void ForeachChildComponents<T>(this Component obj, Action<T> action) where T : Component {
            foreach (T component in obj.GetComponentsInChildren<T>()) {
                action(component);
            }
        }

        public static void ForeachChildComponents<T>(this GameObject obj, Action<T> action) where T : Component {
            foreach (T component in obj.GetComponentsInChildren<T>()) {
                action(component);
            }
        }

        public static T Get<T>(this GameObject gameObject) {
            return gameObject.GetComponent<T>();
        }

        public static T Get<T>(this Component component) {
            return component.GetComponent<T>();
        }

        public static T Add<T>(this GameObject gameObject) where T : Component {
            return gameObject.AddComponent<T>();
        }

        public static T Add<T>(this Component component) where T : Component {
            return component.gameObject.AddComponent<T>();
        }

        public static T GetOrAdd<T>(this GameObject gameObject) where T : Component {
            return GetOrAddComponent<T>(gameObject);
        }

        public static T GetOrAdd<T>(this Component component) where T : Component {
            return GetOrAddComponent<T>(component);
        }

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component {
            T result = gameObject.GetComponent<T>();
            if (result == null)
                result = gameObject.gameObject.AddComponent<T>();

            return result;
        }

        public static T GetOrAddComponent<T>(this Component component) where T : Component {
            T result = component.GetComponent<T>();
            if (result == null)
                result = component.gameObject.AddComponent<T>();

            return result;
        }

        public static Vector3 AddRandomOffset(this Vector3 vector, Vector3 range) {
            return new Vector3(vector.x + Random.Range(-range.x * 0.5f, range.x * 0.5f),
                vector.y + Random.Range(-range.y * 0.5f, range.y * 0.5f),
                vector.z + Random.Range(-range.z * 0.5f, range.z * 0.5f));
        }

        public static int Depth(this Transform element) {
            int depth = 0;
            while (element.parent != null) {
                depth++;
                element = element.parent;
            }

            return depth;
        }
    }
}