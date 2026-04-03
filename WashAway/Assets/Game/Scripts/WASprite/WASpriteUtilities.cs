using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WASpriteUtilities
{
    private static string RENDERER = "Renderer";

    /// <summary>
    /// Safe method to create a WASprite with automatically set up Sprite Renderer children.
    /// </summary>
    /// <returns></returns>
    public static GameObject CreateWASprite()
    {
        GameObject gameObject = new GameObject("WASprite");
        Undo.RegisterCreatedObjectUndo(gameObject, "Created " + gameObject.name);

        WASprite waSprite = Undo.AddComponent<WASprite>(gameObject);

        waSprite.colorRenderer = new SpriteComponentViewer(CreateRenderer(gameObject, "Color"));
        waSprite.normalRenderer = new SpriteComponentViewer(CreateRenderer(gameObject, "Normal"));
        waSprite.roughRenderer = new SpriteComponentViewer(CreateRenderer(gameObject, "Rough"));
        waSprite.thickRenderer = new SpriteComponentViewer(CreateRenderer(gameObject, "Thick"));

        return gameObject;
    }

    /// <summary>
    /// Safe method to create a new Game Object with a Sprite Renderer component.
    /// </summary>
    /// <param name="root">Parent of the new Game Object.</param>
    /// <param name="layer">Default layer for the Sprite Renderer.</param>
    /// <returns></returns>
    public static SpriteRenderer CreateRenderer(GameObject root, string layer)
    {
        // We will be creating a new object to hold the renderer component.
        string rendererObjectName = char.ToUpper(layer[0]) + layer.Substring(1) + RENDERER;

        GameObject rendererObject = new GameObject(rendererObjectName);
        Undo.RegisterCreatedObjectUndo(rendererObject, "Created Renderer Object");

        Undo.RecordObject(rendererObject, "Set Layer");
        rendererObject.layer = LayerMask.NameToLayer(layer);

        SpriteRenderer rendererComponent = Undo.AddComponent<SpriteRenderer>(rendererObject);

        // This code demonstrates how to add a child to a prefab asset.
        // Saving in case I want to reference later.
        /*
        if (PrefabUtility.IsPartOfPrefabAsset(root))
        {
            // Add child object to prefab asset
            string prefabAssetPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(root);

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabAssetPath);
                
            // You might want to look into PrefabUtility.LoadPrefabContents here https://docs.unity3d.com/6000.0/Documentation/ScriptReference/PrefabUtility.LoadPrefabContents.html
            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            Undo.RegisterCreatedObjectUndo(instance, "Created Instance");

            Undo.SetTransformParent(rendererObject.transform, instance.transform, "Set New Parent");

            PrefabUtility.ApplyPrefabInstance(instance, InteractionMode.UserAction);

            Undo.DestroyObjectImmediate(instance);
        }
        else
        */

        Undo.SetTransformParent(rendererObject.transform, root.transform, "Set New Parent");

        return rendererComponent;
    }
}
