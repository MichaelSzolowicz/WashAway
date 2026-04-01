using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WASpriteUtilities
{
    private static string SPRITE = "Sprite";
    private static string RENDERER = "Renderer";
    private static string RENDERER_SPRITE_PROPERTY = "m_Sprite";

    /*
    public static LayerRenderer CreateRenderer(GameObject root, string layer)
    {
        // TODO: Should implement search for already exisitng child objects?

        // We will be creating a new object to hold the renderer component.
        string rendererObjectName = char.ToUpper(layer[0]) + layer.Substring(1) + RENDERER;

        GameObject rendererObject = new GameObject(rendererObjectName);
        Undo.RegisterCreatedObjectUndo(rendererObject, "Created Renderer Object");

        Undo.RecordObject(rendererObject, "Set Layer");
        rendererObject.layer = LayerMask.NameToLayer(layer);

        SpriteRenderer rendererComponent = Undo.AddComponent<SpriteRenderer>(rendererObject);

        // Editing a prefab asset?
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
        {
            // Editing a object in scene
            rendererObject.transform.SetParent(root.transform, false);
        }

        return new LayerRenderer(layer, rendererComponent);
    }
    */
}
