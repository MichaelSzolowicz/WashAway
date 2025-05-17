using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LinePoint
{
    private LinePoint _next;

    [SerializeField] private Vector2 _position;
    private bool _flipNormal;

    public LinePoint Next
    {

        get { return _next; }
        set { _next = value; }
    }

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

    public Vector3 Normal
    {
        get
        {
            if(_next == null)
            {
                return Vector3.zero;
            }

            int direction = _flipNormal ? -1 : 1;
            return Quaternion.Euler(0, 0, -90 * direction) * (_next.Position - _position).normalized; 
        }
    }

    public bool FlipNormal
        { get { return _flipNormal; } set { _flipNormal = value; } }

    public Vector3 GetNormalUnflipped()
    {
        if (_next == null)
        {
            return Vector3.zero;
        }
        return Quaternion.Euler(0, 0, -90) * (_next.Position - _position).normalized;
    }
}
