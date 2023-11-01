using UnityEditor;
using UnityEngine;

public class SphereGizmo : MonoBehaviour {
#if UNITY_EDITOR
    public Color color = Color.cyan;
    public float radius = 0.3f;
    public bool isFilled = true;

    public bool showName = true;
    public string displayName;

    public bool useMatrixScale;

    private void OnDrawGizmos() {
        Gizmos.color = color;

        //Matrix4x4 tempMatrix = Gizmos.matrix;
        if (useMatrixScale) {
            Gizmos.matrix = transform.localToWorldMatrix;

            if (isFilled)
                Gizmos.DrawSphere(Vector3.zero, 0.5f);
            else
                Gizmos.DrawWireSphere(Vector3.zero, 0.5f);
        } else {
            if (isFilled)
                Gizmos.DrawSphere(transform.position, radius);
            else
                Gizmos.DrawWireSphere(transform.position, radius);
        }

        //Gizmos.matrix = tempMatrix;

        if (showName)
            Handles.Label(transform.position, string.IsNullOrWhiteSpace(displayName) ? name : displayName);
    }
#endif
}