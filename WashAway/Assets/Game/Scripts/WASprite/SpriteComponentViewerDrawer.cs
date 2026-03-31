using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SpriteComponentViewer))]
public class SpriteComponentViewerDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        //return EditorGUI.GetPropertyHeight(property, label, true);

        if(property.FindPropertyRelative("spriteRenderer").objectReferenceValue == null ||
            !property.isExpanded)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("spriteRenderer"));
        }
        else
        {
            return EditorGUIUtility.singleLineHeight * 3;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        //EditorGUI.PropertyField(position, property, label, true );

        //EditorGUI.EndProperty();

        if (property.FindPropertyRelative("spriteRenderer").objectReferenceValue == null)
        {
            position = EditorGUI.PrefixLabel(position, label, EditorStyles.boldLabel);

            EditorGUI.ObjectField(position, property.FindPropertyRelative("spriteRenderer"), GUIContent.none);
        }
        else
        {
            // Sprite Renderer
            float x = position.x;
            float width = position.width;

            position.height = EditorGUIUtility.singleLineHeight;
            position.width /= 3;

            //position = EditorGUI.PrefixLabel(position, label, EditorStyles.boldLabel);

            property.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(position, property.isExpanded, label);

            EditorGUI.BeginChangeCheck();

            position.x += position.width;
            position.width = width - position.width;

            EditorGUI.ObjectField(position, property.FindPropertyRelative("spriteRenderer"), GUIContent.none);

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

            // Layer
            EditorGUI.indentLevel++;

            position.y += position.height;
            position.x = x;
            position.width = width;

            SerializedObject serializedSpriteRenderer = new SerializedObject(property.FindPropertyRelative("spriteRenderer").objectReferenceValue);
            SerializedObject serializedSpriteRendererObject = new SerializedObject(serializedSpriteRenderer.FindProperty("m_GameObject").objectReferenceValue);

            serializedSpriteRendererObject.FindProperty("m_Layer").intValue = EditorGUI.LayerField(position, serializedSpriteRendererObject.FindProperty("m_Layer").displayName, serializedSpriteRendererObject.FindProperty("m_Layer").intValue);

            position.y += position.height;

            EditorGUI.PropertyField(position, serializedSpriteRenderer.FindProperty("m_Sprite"));

            serializedSpriteRenderer.ApplyModifiedProperties();
            serializedSpriteRendererObject.ApplyModifiedProperties();

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndFoldoutHeaderGroup();
        EditorGUI.EndProperty();
    }
}
