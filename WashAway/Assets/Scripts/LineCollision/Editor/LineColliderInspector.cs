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

            Handles.color = Color.red;
            Handles.DrawSolidDisc(position, -Vector3.forward, HandleUtility.GetHandleSize(position) * .1f);

            Handles.color = Color.clear;
            Vector3 newPosition = Handles.Slider2D(position, Vector3.forward, Vector3.right, Vector3.up, HandleUtility.GetHandleSize(position) * .1f, Handles.CircleHandleCap, 0);

            if (position != newPosition)
            {
                lineCollider.SetPointWorldPosition(i, newPosition);
            }
        }
    }
}
