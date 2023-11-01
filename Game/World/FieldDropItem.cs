using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ArcaneSurvivorsClient.Game {
    public class FieldDropItem : MonoBehaviour {
        public GameObject triggerArea;

        public float amount;

        public bool canMerge;

        private bool isMerged;
        private int debugId;

        private void Awake() {
            debugId = Random.Range(0, 10000);
            triggerArea.SetActive(false);
        }

        private void Start() {
            StartCoroutine(ActiveRoutine());
        }

        private IEnumerator ActiveRoutine() {
            yield return new WaitForSeconds(0.5f);

            if (triggerArea != null)
                triggerArea.SetActive(true);
        }

        private void OnTriggerEnter(Collider other) {
            if (!canMerge) return;
            if (other.attachedRigidbody == null) return;
            if (!other.attachedRigidbody.CompareTag(gameObject.tag)) return;

            FieldDropItem otherItem = other.attachedRigidbody.GetComponent<FieldDropItem>();
            if (otherItem == null || isMerged || !triggerArea.activeSelf || !otherItem.triggerArea.activeSelf) return;

            otherItem.isMerged = true;
            Debug.Log(
                $"[Gold merged] [{debugId} + {otherItem.debugId}] {amount} + {otherItem.amount} = {amount + otherItem.amount}");
            amount += otherItem.amount;
            Destroy(otherItem.gameObject);
        }
    }
}