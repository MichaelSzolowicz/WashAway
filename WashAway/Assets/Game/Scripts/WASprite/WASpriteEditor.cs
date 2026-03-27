using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WASprite))]
public class WASpriteEditor : Editor
{
    private const int SPRITE_STRING_LENGTH = 6;
    private const string SPRITE = "Sprite";

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
            string checkSubname = nextProperty.name.Length >= 6 ? 
                nextProperty.name.Substring(nextProperty.name.Length - SPRITE_STRING_LENGTH, SPRITE_STRING_LENGTH) : 
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

    }

    private void HandleSprite(SerializedProperty sprite)
    {

    }
}
