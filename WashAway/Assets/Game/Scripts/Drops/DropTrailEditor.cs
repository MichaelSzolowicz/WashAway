using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DropTrail))]
public class DropTrailEditor : Editor
{
    private DropTrail dropTrail;

    private void OnEnable()
    {
        dropTrail = (DropTrail)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var nextProperty = serializedObject.GetIterator();
        nextProperty.Next(true);

        EditorGUI.BeginChangeCheck();
        while (nextProperty.NextVisible(false))
        {
            EditorGUILayout.PropertyField(nextProperty);
        }

        serializedObject.ApplyModifiedProperties();

        if (EditorGUI.EndChangeCheck())
        {
            dropTrail.BuildMeshData();
            dropTrail.BuildPropertyBlock();
        }
    }
}
