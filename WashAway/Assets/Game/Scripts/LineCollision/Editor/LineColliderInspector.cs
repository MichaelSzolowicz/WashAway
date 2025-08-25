using System.Drawing.Printing;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

[CustomEditor(typeof(LineCollider)), CanEditMultipleObjects]
public class LineColliderInspector : Editor
{
    private LineCollider lineCollider;
    private SerializedObject serializedLineCollider;

    private void OnEnable()
    {
        lineCollider = (LineCollider)target;
        serializedLineCollider = new SerializedObject(lineCollider);
        for (int i = 0; i < targets.Length; i++)
        {
            ((LineCollider)targets[i]).IsSelected = true;
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if(targets.Length == 1 )
        {
            int numPoints = serializedObject.FindProperty("points").arraySize;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("points"));

            if (serializedObject.ApplyModifiedProperties())
            {
                int newNumPoints = serializedObject.FindProperty("points").arraySize;
                if (numPoints < newNumPoints)
                {
                    Debug.Log("Added points");

                    for(int i = numPoints; i < newNumPoints; i++)
                    {
                        Vector2 newPointPosition = Vector3.zero;

                        if(i > 1)
                        {
                            Vector3 p1 = serializedObject.FindProperty("points").GetArrayElementAtIndex(i - 2).FindPropertyRelative("position").vector2Value;
                            Vector3 p2 = serializedObject.FindProperty("points").GetArrayElementAtIndex(i - 1).FindPropertyRelative("position").vector2Value;

                            newPointPosition = p2 + (p2 - p1).normalized;
                        }
                        else if (i > 0)
                        {
                            Vector3 p1 = serializedObject.FindProperty("points").GetArrayElementAtIndex(i - 1).FindPropertyRelative("position").vector2Value;

                            newPointPosition = p1 + Vector3.right;
                        }

                        serializedObject.FindProperty("points").GetArrayElementAtIndex(i).FindPropertyRelative("position").vector2Value = newPointPosition;
                    }
                }

                for (int i = 0; i < serializedObject.FindProperty("points").arraySize; i++)
                {
                    SanitizePoint(serializedObject, i);
                }
            }
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("appearance"));

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        serializedLineCollider.Update();

        Handles.color = lineCollider.SelectedColor;
        for (int i = 0; i < lineCollider.NumPoints; i++)
        {
            Vector3 position = serializedLineCollider.FindProperty("points").GetArrayElementAtIndex(i).FindPropertyRelative("position").vector2Value;
            position = lineCollider.transform.position + position;

            Vector3 newPosition = Handles.Slider2D(position, Vector3.forward, Vector3.right, Vector3.up, HandleUtility.GetHandleSize(position) * .1f, Handles.CircleHandleCap, 0);
        
            if(position != newPosition)
            {
                Undo.RecordObject(lineCollider, "Move line collider point");
                SetPointWorldPosition(serializedLineCollider, i, newPosition);
                serializedLineCollider.ApplyModifiedProperties();
            }
        }
    }

    private void SetPointWorldPosition(SerializedObject serializedObject, int index, Vector3 worldPosition)
    {
        var serializedPosition = serializedObject.FindProperty("points").GetArrayElementAtIndex(index).FindPropertyRelative("position");

        serializedPosition.vector2Value = worldPosition - lineCollider.transform.position;

        SanitizePoint(serializedObject, index);
    }

    private void SanitizePoint(SerializedObject serializedObject, int index)
    {
        var serializedPoints = serializedObject.FindProperty("points");
        if (serializedPoints.arraySize > index + 1)
        {
            UpdateNormal(serializedPoints, index);
        }
        if(index - 1 >= 0)
        {
            UpdateNormal(serializedPoints, index - 1);
        }
    }

    private void UpdateNormal(SerializedProperty serializedPoints, int index)
    {
        if (serializedPoints.arraySize <= index) return;

        var p1 = serializedPoints.GetArrayElementAtIndex(index).FindPropertyRelative("position");
        var p2 = serializedPoints.GetArrayElementAtIndex(index + 1).FindPropertyRelative("position");

        Vector2 normal = Quaternion.Euler(0, 0, 90) * (p2.vector2Value - p1.vector2Value).normalized;
        if (Vector2.Dot(normal, Vector2.up) < 0) normal *= -1;

        serializedPoints.GetArrayElementAtIndex(index).FindPropertyRelative("normal").vector2Value = normal;
    }

    private void OnDisable()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            ((LineCollider)targets[i]).IsSelected = false;
        }
    }
}
