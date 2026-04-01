using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WASpriteUtilities
{
    private static string RENDERER = "Renderer";

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
