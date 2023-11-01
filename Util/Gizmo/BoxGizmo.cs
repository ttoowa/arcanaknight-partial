using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BoxGizmo : MonoBehaviour {
#if UNITY_EDITOR
    public Color color = Color.cyan;

    public bool isFilled = false;

    public bool showName = false;
    public string displayName;

    private void OnDrawGizmos() {
        Gizmos.color = color;

        //Matrix4x4 tempMatrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;
        if (isFilled)
            Gizmos.DrawCube(Vector3.zero, Vector3.one);
        else
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

        //Gizmos.matrix = tempMatrix;

        if (showName)
            Handles.Label(transform.position, string.IsNullOrWhiteSpace(displayName) ? name : displayName);
    }
#endif
}