using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Attempts to automatically assign sprite assets to renderers by matching the asset file suffix
/// to a sprite renderer property name.
/// </summary>
[CustomEditor(typeof(WASprite))]
public class WASpriteEditor : Editor
{
    private const string RENDERER = "Renderer";
    private const string SCRIPT_PROP_NAME = "m_Script";
    private const string MULTI_SPRITE_MESSAGE = "Auto Assign Sprites";
    private const string RENDERER_PROP_NAME = "spriteRenderer";
    private const string GAME_OBJECT_PROP_NAME = "m_GameObject";

    private WASprite waSprite;

    private void OnEnable()
    {
        waSprite = (WASprite)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var nextProperty = serializedObject.GetIterator();
        nextProperty.Next(true);

        EditorGUI.BeginChangeCheck();

        while (nextProperty.NextVisible(false))
        {
            switch (nextProperty.name)
            {
                case SCRIPT_PROP_NAME:
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.PropertyField(nextProperty);
                    EditorGUI.EndDisabledGroup();
                    MultiSpriteField();
                    break;
                default:
                    EditorGUILayout.PropertyField(nextProperty);
                    break;
            }
        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
        }

        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Draws a field the user can drag and drop multiple sprites to. 
    /// </summary>
    private void MultiSpriteField()
    {
        Rect multiSpriteField = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight * 1.5f);

        GUI.Box(multiSpriteField, MULTI_SPRITE_MESSAGE);

        Event currentEvent = Event.current;

        if(currentEvent.type == EventType.DragUpdated)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            return;
        }

        if (currentEvent.type != EventType.DragPerform) return;

        if (!multiSpriteField.Contains(currentEvent.mousePosition)) return;

        DragAndDrop.AcceptDrag();

        foreach (string path in DragAndDrop.paths)
        {
            TryLoadSprite(path);
        }

        GUILayout.Space(EditorGUIUtility.singleLineHeight);
    }

    /// <summary>
    /// Tries to automatically assign sprites to the correct Sprite Renderers by matching the end
    /// of the dragged asset file name to a render layer (eg Color, Normal, etc.).
    /// </summary>
    /// <param name="path"></param>
    private void TryLoadSprite(string path)
    {
        string checkPropLayer = "";
        string checkPropSubname = "";
        string fileSuffix = "";
        string filename = Path.GetFileNameWithoutExtension(path);

        filename = WAStringUtility.StripNonAlphabeticSuffix(filename);

        SerializedProperty nextProperty = serializedObject.GetIterator();
        nextProperty.Next(true);

        while(nextProperty.NextVisible(false))
        {
            if (nextProperty.FindPropertyRelative(RENDERER_PROP_NAME) == null) continue;
            if (nextProperty.FindPropertyRelative(RENDERER_PROP_NAME).objectReferenceValue == null) continue;
            if (nextProperty.name.Length < RENDERER.Length) continue;

            checkPropLayer = nextProperty.name.Substring(0, nextProperty.name.Length - RENDERER.Length);

            if (filename.Length < checkPropLayer.Length) continue;

            checkPropSubname = nextProperty.name.Substring(nextProperty.name.Length - RENDERER.Length);

            fileSuffix = filename.Substring(filename.Length - checkPropLayer.Length);

            fileSuffix = char.ToUpper(fileSuffix[0]) + fileSuffix.Substring(1).ToLower();
            checkPropLayer = char.ToUpper(checkPropLayer[0]) + checkPropLayer.Substring(1).ToLower();

            if (checkPropSubname == RENDERER &&
                fileSuffix == checkPropLayer)
            {
                Sprite newValue = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                if (newValue == null)
                {
                    Debug.LogWarning("Could not load sprite asset at path \"" + path + "\"");
                    return;
                }

                UpdateRenderer(nextProperty, newValue);
            }
        }
    }

    /// <summary>
    /// Tries to locate a renderer property matching layer, then assigns a new sprite to it.
    /// Creates a new renderer if the property object reference value is null.
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="newValue"></param>
    private void UpdateRenderer(SerializedProperty renderer, Sprite newSprite)
    {
        string layerName = renderer.name.Length >= RENDERER.Length ?
            renderer.name.Substring(0, renderer.name.Length - RENDERER.Length) : "";
        layerName = char.ToUpper(layerName[0]) + layerName.Substring(1).ToLower();

        int layer = LayerMask.NameToLayer(layerName);

        SerializedObject serializedSpriteRenderer = new SerializedObject(renderer.FindPropertyRelative(RENDERER_PROP_NAME).objectReferenceValue);
        SerializedObject serializedSpriteRendererObject = new SerializedObject(serializedSpriteRenderer.FindProperty(GAME_OBJECT_PROP_NAME).objectReferenceValue);

        serializedSpriteRenderer.FindProperty("m_Sprite").objectReferenceValue = newSprite;
        serializedSpriteRendererObject.FindProperty("m_Layer").intValue = layer;

        serializedSpriteRenderer.ApplyModifiedProperties();
        serializedSpriteRendererObject.ApplyModifiedProperties();

        // A component not attached to the inspected object changed, so tell the editor the inspected object changed to.
        // Ensures prefab instances update immediately when a prefab asset is updated via the project window.
        EditorUtility.SetDirty(target);
    }
}
