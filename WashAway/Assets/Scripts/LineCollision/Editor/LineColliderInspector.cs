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
        lineCollider.IsSelected = true;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }

    private void OnSceneGUI()
    {
        Handles.color = Color.clear;
        for (int i = 0; i < lineCollider.Length; i++)
        {
            Vector3 position = lineCollider.GetPointWorldSpace(i);

            EditorGUI.BeginChangeCheck();
            Vector3 newPosition = Handles.Slider2D(position, Vector3.forward, Vector3.right, Vector3.up, HandleUtility.GetHandleSize(position) * .1f, Handles.CircleHandleCap, 0);

            if (EditorGUI.EndChangeCheck())
            {
                if (position != newPosition)
                {
                    Undo.RecordObject(lineCollider, "Move Line Collider Point");
                    lineCollider.SetPointWorldPosition(i, newPosition);
                }
            }
        }
    }

    private void OnDisable()
    {
        lineCollider.IsSelected = false;
    }
}
