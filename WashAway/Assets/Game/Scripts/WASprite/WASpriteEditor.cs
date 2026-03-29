using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WASprite))]
public class WASpriteEditor : Editor
{
    private const string SPRITE = "Sprite";
    private const string RENDERER = "Renderer";

    private WASprite waSprite;

    private void OnEnable()
    {
        waSprite = (WASprite)target;

        waSprite.UpdateAllRenderers();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        HandleMultiSprite();

        string checkSubname = "";

        var nextProperty = serializedObject.GetIterator();
        nextProperty.Next(true);

        while (nextProperty.NextVisible(false))
        {
            checkSubname = nextProperty.name.Length >= SPRITE.Length ? 
                nextProperty.name.Substring(nextProperty.name.Length - SPRITE.Length, SPRITE.Length) : 
                nextProperty.name;

            if (checkSubname == SPRITE)
            {
                HandleSprite(nextProperty);
                continue;
            }

            switch (nextProperty.name)
            {
                case "m_Script":
                    break;
                default:
                    EditorGUILayout.PropertyField(nextProperty);
                    break;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void HandleMultiSprite()
    {
        Rect multiSpriteField = EditorGUILayout.GetControlRect();

        GUI.Box(multiSpriteField, "Auto assign sprites");

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
            TryLoadSpriteLayerFromPath(path);
        }
    }

    private void TryLoadSpriteLayerFromPath(string path)
    {
        string layer = "";
        SerializedProperty spriteProperty;

        if (!FindSpritePropertyByPathSuffix(path, out layer, out spriteProperty)) return;

        Sprite newValue = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (newValue == null)
        {
            Debug.LogWarning("Could not load sprite asset at path \"" + path + "\"");
            return;
        }

        spriteProperty.objectReferenceValue = newValue;

        UpdateRendererByLayer(layer, newValue);
    }

    private bool FindSpritePropertyByPathSuffix(string path, out string layer, out SerializedProperty serializedProperty)
    {
        string filename = Path.GetFileNameWithoutExtension(path);

        layer = "";

        serializedProperty = serializedObject.GetIterator();
        serializedProperty.Next(true);

        while (serializedProperty.NextVisible(true))
        {
            layer = serializedProperty.name.Length >= SPRITE.Length ?
                serializedProperty.name.Substring(0, serializedProperty.name.Length - SPRITE.Length) : "";

            // TODO: Strip non-alphabetic characters from end of filename so animated sprites can be supported.

            if (filename.Length >= layer.Length && 
                filename.Substring(filename.Length - layer.Length) == layer)
            {
                return true;
            }
        }

        serializedProperty = null;
        return false;
    }

    private void HandleSprite(SerializedProperty spriteProperty)
    {
        Object oldValue = spriteProperty.objectReferenceValue;

        EditorGUILayout.PropertyField(spriteProperty);

        Object newValue = spriteProperty.objectReferenceValue;

        if (oldValue == newValue) return;

        string layer = spriteProperty.name.Substring(0, spriteProperty.name.Length - SPRITE.Length);

        UpdateRendererByLayer(layer, (Sprite)newValue);
    }

    private void UpdateRendererByLayer(string layer, Sprite newValue)
    {
        string rendererPropertyName = layer + RENDERER;

        SerializedProperty rendererProperty = serializedObject.FindProperty(rendererPropertyName);
        if (rendererProperty == null)
        {
            Debug.LogError("No serialized property \"" + layer + SPRITE + "\" found on serialized object \"" + serializedObject.targetObject.name + "\"");
            return;
        }

        if (rendererProperty.objectReferenceValue != null)
        {
            // Case: renderer already set up, safe to assign sprite.
            SerializedObject so = new SerializedObject(rendererProperty.objectReferenceValue);
            so.FindProperty("m_Sprite").objectReferenceValue = newValue;
            so.ApplyModifiedProperties();
            return;
        }

        // Case: no renderer found, create one.
        CreateRenderer(rendererProperty, (Sprite)newValue);
    }

    private void CreateRenderer(SerializedProperty rendererProperty, Sprite newValue)
    {
        // TODO: Should implement search for already exisitng child objects?

        // We will be creating a new object to hold the renderer component.
        string rendererPropertyName = rendererProperty.name;
        string rendererObjectName = char.ToUpper(rendererPropertyName[0]) + rendererPropertyName.Substring(1);
        string layer = rendererObjectName.Substring(0, rendererObjectName.Length - RENDERER.Length);

        GameObject rendererObject = new GameObject(rendererObjectName);
        rendererObject.layer = LayerMask.NameToLayer(layer);
        Undo.RegisterCreatedObjectUndo(rendererObject, "Created Renderer Object");

        SpriteRenderer rendererComponent = rendererObject.AddComponent<SpriteRenderer>();
        rendererComponent.sprite = newValue;

        // Editing a prefab asset?
        if (PrefabUtility.IsPartOfPrefabAsset(waSprite))
        {
            // Add child object to prefab asset
            string prefabAssetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(waSprite);

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabAssetPath);
            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            Undo.RegisterCreatedObjectUndo(instance, "Created instance");

            rendererObject.transform.SetParent(instance.transform, false);

            PrefabUtility.ApplyPrefabInstance(instance, InteractionMode.UserAction);
            DestroyImmediate(instance);

            // Link renderer to wasprite
            Transform rendererTransform = prefab.transform.Find(rendererObjectName);
            if (rendererTransform == null)
            {
                Debug.LogError("Could not find child object of name \"" + rendererObjectName + "\" in prefab at \"" + prefabAssetPath + "\"");
                return;
            }

            rendererComponent = rendererTransform.GetComponent<SpriteRenderer>();
            if (rendererComponent == null)
            {
                Debug.LogError("Could not find component of type \"" + typeof(SpriteRenderer).Name + "\" on child object \"" + rendererTransform.gameObject.name + "\" of prefab at path \"" + prefabAssetPath + "\"");
                return;
            }

            rendererProperty.objectReferenceValue = rendererComponent;

            return;
        }

        // Editing a object in scene
        rendererObject.transform.SetParent(waSprite.transform, false);

        rendererProperty.objectReferenceValue = rendererComponent;
    }
}
