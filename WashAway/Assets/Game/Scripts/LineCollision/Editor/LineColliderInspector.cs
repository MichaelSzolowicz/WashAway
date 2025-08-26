using UnityEditor;
using UnityEngine;

/// <summary>
/// Generates interface for editing line colliders.
/// Uses serialized property to ensure changes are saved and reconstructed properly.
/// </summary>
[CustomEditor(typeof(LineCollider)), CanEditMultipleObjects]
public class LineColliderInspector : Editor
{
    private LineCollider lineCollider;

    private void OnEnable()
    {
        lineCollider = (LineCollider)target;
        for (int i = 0; i < targets.Length; i++)
        {
            ((LineCollider)targets[i]).IsSelected = true;
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var it = serializedObject.GetIterator();
        it.Next(true);

        while (it.NextVisible(false))
        {
            switch (it.name)
            {
                case "points":
                    HandlePoints(it);
                    break;
                case "m_Script":
                    GUI.enabled = false;
                    EditorGUILayout.PropertyField(it);
                    GUI.enabled = true;
                    break;
                default:
                    EditorGUILayout.PropertyField(it);
                    break;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void HandlePoints(SerializedProperty points)
    {
        if (targets.Length != 1) return;

        int numPoints = points.arraySize;

        EditorGUILayout.PropertyField(points);

        if (!serializedObject.ApplyModifiedProperties()) return;

        int newNumPoints = points.arraySize;
        for (int i = numPoints; i < newNumPoints; i++)
        {
            Vector2 newPointPosition = Vector3.zero;

            if (i > 1)
            {
                Vector3 p1 = points.GetArrayElementAtIndex(i - 2).FindPropertyRelative("position").vector2Value;
                Vector3 p2 = points.GetArrayElementAtIndex(i - 1).FindPropertyRelative("position").vector2Value;

                newPointPosition = p2 + (p2 - p1).normalized;
            }
            else if (i > 0)
            {
                Vector3 p1 = points.GetArrayElementAtIndex(i - 1).FindPropertyRelative("position").vector2Value;

                newPointPosition = p1 + Vector3.right;
            }

            points.GetArrayElementAtIndex(i).FindPropertyRelative("position").vector2Value = newPointPosition;
        }

        SanitizePoints(points);
    }

    private void OnSceneGUI()
    {
        var slc = new SerializedObject(lineCollider);

        Handles.color = lineCollider.SelectedColor;
        for (int i = 0; i < lineCollider.NumPoints; i++)
        {
            var points = slc.FindProperty("points");
            var position = points.GetArrayElementAtIndex(i).FindPropertyRelative("position");

            Vector3 drawPos = position.vector2Value;
            drawPos = drawPos + lineCollider.transform.position;

            Vector3 newPosition = Handles.Slider2D(drawPos, Vector3.forward, Vector3.right, 
                Vector3.up, HandleUtility.GetHandleSize(drawPos) * .1f, Handles.CircleHandleCap, 0);
        
            if(drawPos != newPosition)
            {
                position.vector2Value = newPosition - lineCollider.transform.position;
                SanitizePoint(points, i);
                slc.ApplyModifiedProperties();
            }
        }
    }

    private void SanitizePoints(SerializedProperty points)
    {
        for (int i = 0; i < points.arraySize; i++)
        {
            SanitizePoint(points, i);
        }
    }

    private void SanitizePoint(SerializedProperty points, int index)
    {
        if (points.arraySize > index + 1)
        {
            UpdateNormal(points, index);
        }
        if (index - 1 >= 0)
        {
            UpdateNormal(points, index - 1);
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
