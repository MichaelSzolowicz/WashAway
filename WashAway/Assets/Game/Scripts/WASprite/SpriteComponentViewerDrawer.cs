using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[CustomPropertyDrawer(typeof(SpriteComponentViewer))]
public class SpriteComponentViewerDrawer : PropertyDrawer
{
    private SerializedProperty m_Property;

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
        m_Property = property;

        Rect originalPosition = position;

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

            CheckDragAndDrop(originalPosition, serializedSpriteRendererObject, serializedSpriteRenderer);

            serializedSpriteRenderer.ApplyModifiedProperties();
            serializedSpriteRendererObject.ApplyModifiedProperties();

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndFoldoutHeaderGroup();
        EditorGUI.EndProperty();
    }

    private void CheckDragAndDrop(Rect position, SerializedObject rendererObject, SerializedObject renderer)
    {
        Event currentEvent = Event.current;

        if (currentEvent.type == EventType.DragUpdated)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            return;
        }

        if (currentEvent.type != EventType.DragPerform) return;

        if (!position.Contains(currentEvent.mousePosition)) return;

        Debug.Log("Accept drag");

        foreach(string path in DragAndDrop.paths)
        {
            if(m_Property.isArray)
            {
                for(int i = 0; i < m_Property.arraySize; i++)
                {
                    SerializedObject serializedSpriteRenderer = new SerializedObject(m_Property.GetArrayElementAtIndex(i).FindPropertyRelative("spriteRenderer").objectReferenceValue);
                    SerializedObject serializedSpriteRendererObject = new SerializedObject(serializedSpriteRenderer.FindProperty("m_GameObject").objectReferenceValue);

                    TryLoadSprite(path, serializedSpriteRendererObject, serializedSpriteRenderer);

                    serializedSpriteRenderer.ApplyModifiedProperties();
                    serializedSpriteRendererObject.ApplyModifiedProperties();
                }
            }
            else
            {
                TryLoadSprite(path, rendererObject, renderer);
            }

        }

        DragAndDrop.AcceptDrag();
    }

    private void TryLoadSprite(string path, SerializedObject rendererObject, SerializedObject renderer)
    {
        string filename = Path.GetFileNameWithoutExtension(path);

        // strip numbers
        int lastLetter = filename.Length - 1;
        for(; lastLetter >= 0; lastLetter--)
        {
            if (char.IsLetter(filename[lastLetter]))
            {
                break;
            }
        }

        filename = filename.Substring(0, lastLetter + 1);

        int checkLayer = rendererObject.FindProperty("m_Layer").intValue;

        string checkLayerName = LayerMask.LayerToName(checkLayer);

        string checkSubname = filename.Length > checkLayerName.Length ?
            filename.Substring(filename.Length - checkLayerName.Length) : "";

        if (checkSubname == "") return;

        checkSubname = char.ToUpper(checkSubname[0]) + checkSubname.Substring(1).ToLower();

        if(checkSubname != checkLayerName) return;

        Sprite newValue = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (newValue == null)
        {
            Debug.LogWarning("Could not load sprite asset at path \"" + path + "\"");
            return;
        }

        renderer.FindProperty("m_Sprite").objectReferenceValue = newValue;
    }
}
