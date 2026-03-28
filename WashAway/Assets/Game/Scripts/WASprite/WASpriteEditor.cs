using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[CustomEditor(typeof(WASprite))]
public class WASpriteEditor : Editor
{
    private const string SPRITE = "Sprite";
    private const string RENDERER = "Renderer";

    private WASprite waSprite;

    private void OnEnable()
    {
        waSprite = (WASprite)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        HandleMultiSprite();

        var nextProperty = serializedObject.GetIterator();
        nextProperty.Next(true);

        while (nextProperty.NextVisible(false))
        {
            string checkSubname = nextProperty.name.Length >= SPRITE.Length ? 
                nextProperty.name.Substring(nextProperty.name.Length - SPRITE.Length, SPRITE.Length) : 
                nextProperty.name;

            switch(checkSubname)
            {
                case SPRITE:
                    HandleSprite(nextProperty); 
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

        Event evt = Event.current;

        if(evt.type == EventType.DragUpdated)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            return;
        }

        if (evt.type != EventType.DragPerform) return;

        if (!multiSpriteField.Contains(evt.mousePosition)) return;

        DragAndDrop.AcceptDrag();

        foreach (string path in DragAndDrop.paths)
        {
            var nextProperty = serializedObject.GetIterator();
            nextProperty.Next(true);

            Debug.Log(path);

            while (nextProperty.NextVisible(true))
            {
                string layer = nextProperty.name.Length >= 6 ? nextProperty.name.Substring(0, nextProperty.name.Length - SPRITE.Length) : "";
                
                string filename = Path.GetFileNameWithoutExtension(path);

                if(filename.Length >= layer.Length && filename.Substring(filename.Length - layer.Length) == layer)
                {
                    string rendererPropertyName = layer + RENDERER;

                    SerializedProperty rendererProperty = serializedObject.FindProperty(rendererPropertyName);
                    if (rendererProperty == null)
                    {
                        Debug.LogError("No serialized property \"" + layer + SPRITE + "\" found on serialized object \"" + serializedObject.targetObject.name + "\"");
                        continue;
                    }

                    Sprite newValue = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                    if(newValue == null)
                    {
                        Debug.LogWarning("Could not load sprite asset at path \"" + path + "\"");
                    }

                    nextProperty.objectReferenceValue = newValue;

                    if(rendererProperty.objectReferenceValue == null)
                    {
                        CreateRenderer(rendererProperty, newValue);
                    }
                }
            }
        }
    }

    private void HandleSprite(SerializedProperty spriteProperty)
    {
        Object oldValue = spriteProperty.objectReferenceValue;

        EditorGUILayout.PropertyField(spriteProperty);

        Object newValue = spriteProperty.objectReferenceValue;

        if (oldValue == newValue) return;

        string layer = spriteProperty.name.Substring(0, spriteProperty.name.Length - SPRITE.Length);
        string rendererPropertyName = layer + RENDERER;

        SerializedProperty rendererProperty = serializedObject.FindProperty(rendererPropertyName);
        if (rendererProperty == null)
        {
            Debug.LogError("No serialized property \"" + layer + SPRITE + "\" found on serialized object \"" + serializedObject.targetObject.name + "\"");
            return;
        }

        SpriteRenderer rendererComponent = (SpriteRenderer)rendererProperty.objectReferenceValue;

        if(rendererComponent != null)
        {
            // Case: renderer already set up, safe to assign sprite.
            rendererComponent.sprite = (Sprite)newValue;
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

        SpriteRenderer rendererComponent = rendererObject.AddComponent<SpriteRenderer>();
        rendererComponent.sprite = newValue;

        // Editing a prefab asset?
        if (PrefabUtility.IsPartOfPrefabAsset(waSprite))
        {
            // Add child object to prefab asset
            string prefabAssetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(waSprite);

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabAssetPath);
            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

            rendererObject.transform.SetParent(instance.transform, false);

            PrefabUtility.ApplyPrefabInstance(instance, InteractionMode.AutomatedAction);
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
