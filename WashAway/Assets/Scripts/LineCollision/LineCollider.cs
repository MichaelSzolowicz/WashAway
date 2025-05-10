using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCollider : MonoBehaviour
{
    [SerializeField] protected List<Vector2> _points = new List<Vector2>();

    public int Length
    {
        get
        {
            return _points.Count;
        }
    }

    private void Start()
    {
        LineColllisionScene.Instance.RegisterLineCollider(this);
    }

    public Vector3 GetPointWorldSpace(int index)
    {
        if(index < 0 || index >= _points.Count)
        {
            throw new IndexOutOfRangeException("Index out of range.");
        }

        return transform.TransformPoint(_points[index]);

    }

    public void SetPointInWorldSpace(int index, Vector3 worldPoint)
    {
        if (index < 0 || index >= _points.Count)
        {
            throw new IndexOutOfRangeException("Index out of range.");
        }

        _points[index] = transform.InverseTransformPoint(worldPoint);
    }
}
