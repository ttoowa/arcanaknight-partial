using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace ArcaneSurvivorsClient {
    public static class CollectionUtility {
        public static T FindClosest<T>(this IEnumerable<T> collection, Func<T, double> distanceSelector)
            where T : class {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (distanceSelector == null)
                throw new ArgumentNullException(nameof(distanceSelector));

            T closestItem = default;
            double closestDistance = double.MaxValue;

            foreach (T item in collection) {
                double distance = distanceSelector(item);
                if (distance < closestDistance) {
                    closestItem = item;
                    closestDistance = distance;
                }
            }

            return closestItem;
        }

        public static void ForEach<T>(this T[] array, Action<T> action) {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (action == null)
                throw new ArgumentNullException(nameof(action));

            foreach (T item in array) {
                action(item);
            }
        }

        public static T GetRandom<T>(this HashSet<T> hashSet) {
            if (hashSet == null)
                throw new ArgumentNullException(nameof(hashSet));

            int randomIndex = Random.Range(0, hashSet.Count);
            int currentIndex = 0;
            foreach (T item in hashSet) {
                if (currentIndex == randomIndex)
                    return item;

                currentIndex++;
            }

            return default;
        }
    }
}