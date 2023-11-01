using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace ArcaneSurvivorsClient {
    public static class CoroutineUtility {
        public static Coroutine AsCoroutine<T>(this Task<T> task) {
            return CoroutineDispatcher.Instance.StartCoroutine(WaitTaskRoutine(task));
        }

        public static Coroutine AsCoroutine(this Task task) {
            return CoroutineDispatcher.Instance.StartCoroutine(WaitTaskRoutine(task));
        }

        private static IEnumerator WaitTaskRoutine(Task task) {
            while (!task.IsCompleted) {
                yield return null;
            }
        }

        public static IEnumerator WaitBoolRoutine(this Func<bool> predicate) {
            while (!predicate()) {
                yield return null;
            }
        }
    }
}