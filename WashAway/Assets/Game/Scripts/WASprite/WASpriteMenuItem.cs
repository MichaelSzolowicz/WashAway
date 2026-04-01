using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WASpriteMenuItem
{
    [MenuItem("GameObject/WashAway/WASprite")]
    private static void CreateWASprite(MenuCommand menuCommand)
    {
        GameObject gameObject = WASpriteUtilities.CreateWASprite();
        GameObjectUtility.SetParentAndAlign(gameObject, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(gameObject, "Created " + gameObject.name);
        Selection.activeObject = gameObject;
    }
}
