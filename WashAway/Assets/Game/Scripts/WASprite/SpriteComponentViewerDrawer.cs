using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[CustomPropertyDrawer(typeof(SpriteComponentViewer))]
public class SpriteComponentViewerDrawer : PropertyDrawer
{

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if(property.FindPropertyRelative("spriteRenderer").objectReferenceValue == null ||
            !property.isExpanded)
        {
            return EditorGUIUtility.singleLineHeight;
        }
        else
        {
            return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 3;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        if (property.FindPropertyRelative("spriteRenderer").objectReferenceValue == null)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            position = EditorGUI.PrefixLabel(position, label, EditorStyles.boldLabel);

            EditorGUI.ObjectField(position, property.FindPropertyRelative("spriteRenderer"), GUIContent.none);
        }
        else
        {
            Rect usePosition = new Rect(position.x, position.y,
                EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);

            property.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(usePosition, property.isExpanded, label);

            EditorGUI.BeginChangeCheck();

            usePosition.x += usePosition.width;
            usePosition.width = position.width - usePosition.width;

            EditorGUI.ObjectField(usePosition, property.FindPropertyRelative("spriteRenderer"), GUIContent.none);

            if(EditorGUI.EndChangeCheck() && property.FindPropertyRelative("spriteRenderer").objectReferenceValue == null)
            {
                EditorGUI.EndFoldoutHeaderGroup();
                return;
            }

            if (!property.isExpanded)
            {
                EditorGUI.EndFoldoutHeaderGroup();
                return;
            }

            EditorGUI.indentLevel++;

            usePosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            usePosition.x = position.x;
            usePosition.width = position.width;

            SerializedObject serializedSpriteRenderer = new SerializedObject(property.FindPropertyRelative("spriteRenderer").objectReferenceValue);
            SerializedObject serializedSpriteRendererObject = new SerializedObject(serializedSpriteRenderer.FindProperty("m_GameObject").objectReferenceValue);

            usePosition = EditorGUI.PrefixLabel(usePosition, new GUIContent(serializedSpriteRendererObject.FindProperty("m_Layer").displayName));
            serializedSpriteRendererObject.FindProperty("m_Layer").intValue = EditorGUI.LayerField(usePosition, GUIContent.none, serializedSpriteRendererObject.FindProperty("m_Layer").intValue);

            usePosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            usePosition.width = position.width;
            usePosition.x = position.x;

            usePosition = EditorGUI.PrefixLabel(usePosition, new GUIContent(serializedSpriteRenderer.FindProperty("m_Sprite").displayName));
            EditorGUI.PropertyField(usePosition, serializedSpriteRenderer.FindProperty("m_Sprite"), GUIContent.none);

            serializedSpriteRenderer.ApplyModifiedProperties();
            serializedSpriteRendererObject.ApplyModifiedProperties();

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndFoldoutHeaderGroup();
        EditorGUI.EndProperty();
    }
}
