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
            Vector3 position = lineCollider.GetPointWorldSpace(i);
            Vector3 newPosition = Handles.PositionHandle(position, lineCollider.transform.rotation);

            if(newPosition != position)
            {
                lineCollider.SetPointInWorldSpace(i, newPosition);
            }
        }

    }
}
