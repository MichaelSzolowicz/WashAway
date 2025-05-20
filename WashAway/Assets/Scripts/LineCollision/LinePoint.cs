using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

[System.Serializable]
public class LinePoint
{
    [SerializeField] public Vector2 position;
    
    [HideInInspector] [SerializeField] public Vector2 normal = Vector2.up;

    public float x
    {
        get { return position.x; }
        set
        {
            position.x = value;
        }
    }

    public float y
    {
        get { return position.y; }
        set
        {
            position.y = value;
        }
    }
}
