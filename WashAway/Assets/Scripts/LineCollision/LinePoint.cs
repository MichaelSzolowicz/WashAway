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
}
