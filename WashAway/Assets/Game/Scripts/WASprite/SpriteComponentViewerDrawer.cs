using UnityEditor;
using UnityEngine;

/// <summary>
/// Searches the Sprite Renderer referenced by Sprite Component Viewer for relevant properties and exposes them in the inspector.
/// </summary>
[CustomPropertyDrawer(typeof(SpriteComponentViewer))]
public class SpriteComponentViewerDrawer : PropertyDrawer
{
    private const string RENDERER_PROP_NAME = "spriteRenderer";
    private const string GAME_OBJECT_PROP_NAME = "m_GameObject";
    private const string SPRITE_PROP_NAME = "m_Sprite";
    private const string LAYER_PROP_NAME = "m_Layer";

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

        if (property.FindPropertyRelative(RENDERER_PROP_NAME).objectReferenceValue == null)
        {
            position.height = EditorGUIUtility.singleLineHeight;

            position = EditorGUI.PrefixLabel(position, label, EditorStyles.boldLabel);

            EditorGUI.ObjectField(position, property.FindPropertyRelative(RENDERER_PROP_NAME), GUIContent.none);
        }
        else
        {
            Rect usePosition = new Rect(position.x, position.y,
                EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight);

            property.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(usePosition, property.isExpanded, label);

            EditorGUI.BeginChangeCheck();

            usePosition.x += usePosition.width;
            usePosition.width = position.width - usePosition.width;

            EditorGUI.ObjectField(usePosition, property.FindPropertyRelative(RENDERER_PROP_NAME), GUIContent.none);

            if(EditorGUI.EndChangeCheck() && property.FindPropertyRelative(RENDERER_PROP_NAME).objectReferenceValue == null)
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

            SerializedObject serializedSpriteRenderer = new SerializedObject(property.FindPropertyRelative(RENDERER_PROP_NAME).objectReferenceValue);
            SerializedObject serializedSpriteRendererObject = new SerializedObject(serializedSpriteRenderer.FindProperty(GAME_OBJECT_PROP_NAME).objectReferenceValue);

            // EditorGUI.LayerField() does not automatically display overrides, so this is wrapped in EditorGUI.BeginProperty()
            EditorGUI.BeginProperty(usePosition, new GUIContent(serializedSpriteRendererObject.FindProperty(LAYER_PROP_NAME).displayName), serializedSpriteRendererObject.FindProperty(LAYER_PROP_NAME));
            usePosition = EditorGUI.PrefixLabel(usePosition, new GUIContent(serializedSpriteRendererObject.FindProperty(LAYER_PROP_NAME).displayName));
            serializedSpriteRendererObject.FindProperty("m_Layer").intValue = EditorGUI.LayerField(usePosition, GUIContent.none, serializedSpriteRendererObject.FindProperty(LAYER_PROP_NAME).intValue);
            EditorGUI.EndProperty();

            usePosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            usePosition.width = position.width;
            usePosition.x = position.x;

            usePosition = EditorGUI.PrefixLabel(usePosition, new GUIContent(serializedSpriteRenderer.FindProperty(SPRITE_PROP_NAME).displayName));
            EditorGUI.PropertyField(usePosition, serializedSpriteRenderer.FindProperty(SPRITE_PROP_NAME), GUIContent.none);

            serializedSpriteRenderer.ApplyModifiedProperties();
            serializedSpriteRendererObject.ApplyModifiedProperties();

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndFoldoutHeaderGroup();
        EditorGUI.EndProperty();
    }
}
