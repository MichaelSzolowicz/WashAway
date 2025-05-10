using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LineCollider))]
public class LineColliderInspector : Editor
{
    private LineCollider lineCollider;

    private void OnEnable()
    {
        lineCollider = (LineCollider)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }

    private void OnSceneGUI()
    {
        for(int i = 0; i < lineCollider.Length; i++)
        {
            lineCollider.SetPointInWorldSpace(i, Handles.PositionHandle(lineCollider.GetPointWorldSpace(i), lineCollider.transform.rotation));
        }
    }

}
