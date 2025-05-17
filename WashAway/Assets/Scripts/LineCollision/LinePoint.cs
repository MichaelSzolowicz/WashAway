using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LinePoint
{
    private LinePoint _next;

    [SerializeField] private Vector2 _position;
    private bool _flipNormal;

    public Vector2 Position
    {
        get
        {
            return _position;
        }
        set
        {
            _position = value;
        }
    }

    public float x
    {
        get { return _position.x; }
        set
        {
            _position.x = value;
        }
    }

    public float y
    {
        get { return _position.y; }
        set
        {
            _position.y = value;
        }
    }
}
