using System.Linq;
using ArcaneSurvivorsClient.Locale;
using Unity.VisualScripting;
using UnityEngine;

namespace ArcaneSurvivorsClient.Game {
    [CreateAssetMenu(fileName = "StageMap", menuName = "ScriptableObject/StageMap", order = 1)]
    public class StageMap : ScriptableObject {
        public SpawnPoint[] PlayerSpawnPoints => mapInstance.GetComponentsInChildren<SpawnPoint>()
            .Where(x => x.gameObject.activeInHierarchy && x.force == PawnForce.Player).ToArray();

        public SpawnPoint[] MonsterSpawnPoints => mapInstance.GetComponentsInChildren<SpawnPoint>()
            .Where(x => x.gameObject.activeInHierarchy && x.force == PawnForce.Monster).ToArray();

        [LocaleKey]
        public string name;

        public GameObject mapPrefab;
        private GameObject mapInstance;

        public void CreateInstance() {
            if (mapInstance != null) return;
            mapInstance = mapPrefab.Instantiate(null, Vector3.zero);
        }

        public void RemoveInstance() {
            if (mapInstance == null) return;
            Destroy(mapInstance);
            mapInstance = null;
        }

        public Vector3 SamplePlayerSpawnPoint() {
            return PlayerSpawnPoints[Random.Range(0, PlayerSpawnPoints.Length)].transform.position;
        }

        public Vector3 SampleMonsterSpawnPoint(bool exceptCamArea = true) {
            SpawnPoint[] spawnPoints = null;
            if (exceptCamArea)
                spawnPoints = MonsterSpawnPoints.Where(x => !IsInCamArea(x)).ToArray();

            if (spawnPoints == null || spawnPoints.Length == 0)
                spawnPoints = MonsterSpawnPoints;

            return spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;

            bool IsInCamArea(SpawnPoint spawnPoint) {
                return Vector3.Distance(spawnPoint.transform.position, Cameras.Instance.WorldCameraFocus) <
                       Cameras.WorldSafeDistance;
            }
        }
    }
}