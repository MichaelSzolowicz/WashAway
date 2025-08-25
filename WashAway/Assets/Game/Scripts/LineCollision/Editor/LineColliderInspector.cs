using UnityEngine;
using UnityEditor;
using UnityEngine.Assertions;
using System.Drawing.Printing;

[CustomEditor(typeof(LineCollider))]
public class LineColliderInspector : Editor
{
    private LineCollider lineCollider;
    private SerializedObject serializedLineCollider;
    private SerializedProperty serializedPoints;

    private void OnEnable()
    {
        lineCollider = (LineCollider)target;
        lineCollider.IsSelected = true;

        Init();
    }

    private void Init()
    {
        string pointsPropertyName = "points";

        serializedLineCollider = CreateSerializedObjectWithAssert(lineCollider);
        serializedPoints = FindPropertyWithAssert(serializedLineCollider, pointsPropertyName);    
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }

    private void OnSceneGUI()
    {
        Init();
        Handles.color = Color.yellow;
        for (int i = 0; i < lineCollider.NumPoints; i++)
        {
            Vector3 position = FindRelativePropertyWithAssert(serializedPoints.GetArrayElementAtIndex(i), "position").vector2Value;
            position = position + lineCollider.transform.position;

            EditorGUI.BeginChangeCheck();

            Vector3 newPosition = Handles.Slider2D(position, Vector3.forward, Vector3.right, Vector3.up, HandleUtility.GetHandleSize(position) * .1f, Handles.CircleHandleCap, 0);

            if (EditorGUI.EndChangeCheck())
            {
                if (position != newPosition)
                {
                    Undo.RecordObject(lineCollider, "Move Line Collider Point");
                    SetPointWorldPosition(i, newPosition);
                    serializedLineCollider.ApplyModifiedProperties();
                }
            }

        }
    }

    private void SetPointWorldPosition(int index, Vector3 worldPosition)
    {
        var serializedPoint = serializedPoints.GetArrayElementAtIndex(index);

        var serializedPosition = FindRelativePropertyWithAssert(serializedPoint, "position");

        serializedPosition.vector2Value = worldPosition - lineCollider.transform.position;

        SanitizePoint(index);
    }

    private void SanitizePoint(int index)
    {
        if (serializedPoints.arraySize > index + 1)
        {
            UpdateNormal(index);
        }
        if(index - 1 >= 0)
        {
            UpdateNormal(index - 1);
        }
    }

    private void UpdateNormal(int index)
    {
        if (serializedPoints.arraySize <= index) return;

        var p1 = FindRelativePropertyWithAssert(serializedPoints.GetArrayElementAtIndex(index), "position");
        var p2 = FindRelativePropertyWithAssert(serializedPoints.GetArrayElementAtIndex (index + 1), "position");

        Vector2 normal = Quaternion.Euler(0, 0, 90) * (p2.vector2Value - p1.vector2Value).normalized;
        if (Vector2.Dot(normal, Vector2.up) < 0) normal *= -1;

        FindRelativePropertyWithAssert(serializedPoints.GetArrayElementAtIndex(index), "normal").vector2Value = normal;
    }

    private void OnDisable()
    {
        lineCollider.IsSelected = false;
    }

    private SerializedObject CreateSerializedObjectWithAssert(Object src)
    {
        var result = new SerializedObject(src);
        string srcTypeName = src.GetType().Name;
        Assert.IsTrue(result != null, "Failed to create serialized object for object of type \"" + srcTypeName + "\"");
        return result;
    }

    private SerializedProperty FindPropertyWithAssert(SerializedObject serializedObject, string propertyName)
    {
        var result = serializedObject.FindProperty(propertyName);
        Assert.IsTrue(result != null, "Could not find property \"" + propertyName + "\" on on serialized object from object type " + serializedObject.targetObject.GetType().Name);
        return result;
    }

    private SerializedProperty FindRelativePropertyWithAssert(SerializedProperty serializedProperty, string propertyName)
    {
        var result = serializedProperty.FindPropertyRelative(propertyName);
        Assert.IsTrue(result != null, "Could not find realtive property \"" + propertyName + "\" on serialized property \"" + serializedProperty.name + "\"");
        return result;
    }
}
