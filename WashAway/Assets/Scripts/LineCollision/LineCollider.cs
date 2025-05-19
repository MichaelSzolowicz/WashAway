using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class LineCollider : MonoBehaviour
{
    [SerializeField] protected List<LinePoint> _points = new List<LinePoint>();
    [HideInInspector] [SerializeField] protected int length = 0;

    [SerializeField] protected float rotation = 0;

    [SerializeField] protected float width = 0;

    [SerializeField] protected bool visibleInGame = true;
    [SerializeField] protected bool visibleInEditor = true;

    [SerializeField] protected Color defaultColor = Color.white;
    [SerializeField] protected Color selectedColor = Color.yellow;
    [SerializeField] protected float pointSize = .01f;
    [SerializeField] protected float lineWidth = 1;

    protected bool isSelected = false;

    [HideInInspector] [SerializeField] private Vector3 previousPosition = Vector3.zero;

    public int Length
    {
        get
        {
            return _points.Count;
        }
    }

    public float Rotation
    {
        get
        {
            return rotation;
        }
    }

    public float Width
    {
        get
        {
            return width;
        }
    }

    public bool IsSelected
    {
        get
        {
            return isSelected;
        }
        set
        {
            isSelected = value;
        }
    }

    private void OnEnable()
    {
        if (Application.isPlaying)
            LineCollisionScene.Instance.RegisterLineCollider(this);
    }

    public LinePoint GetPoint(int index)
    {
        if (index < 0 || index >= _points.Count)
        {
            throw new IndexOutOfRangeException("Index out of range.");
        }

        return _points[index];
    }

    public void SetPointWorldPosition(int index, Vector3 worldPosition)
    {
        if (index < 0 || index >= _points.Count)
        {
            throw new IndexOutOfRangeException("Index out of range.");
        }

        _points[index].Position = worldPosition;

        SetNormal(index);
        if (index - 1 >= 0)
            SetNormal(index - 1);
    }

    protected void OnDisable()
    {
        if (Application.isPlaying && gameObject.scene.isLoaded)
            LineCollisionScene.Instance.RemoveLineCollider(this);
    }

    protected void OnDrawGizmos()
    {
        if (!visibleInEditor && !visibleInGame) return;
        else if (SceneView.currentDrawingSceneView == null && !visibleInGame) return;
        else if(SceneView.currentDrawingSceneView != null && !visibleInEditor) return;

        Color useColor = isSelected ? selectedColor : defaultColor;
        Handles.color = useColor;
        for (int i = 0; i < Length; i++)
        {
            LinePoint point = GetPoint(i);

            Gizmos.DrawIcon(point.Position, "point", false, useColor);
        }

        for (int p1 = 0, p2 = 1; p2 < Length; p1++, p2++)
        {
            LinePoint lineStart = GetPoint(p1);
            LinePoint lineEnd = GetPoint(p2);

            Handles.DrawLine(lineStart.Position, lineEnd.Position, lineWidth);
        }

        Handles.color = Color.white;
        for (int p1 = 0, p2 = 1; p2 < Length; p1++, p2++)
        {
            LinePoint lineStart = GetPoint(p1);
            LinePoint lineEnd = GetPoint(p2);

            Vector3 normalStart = (lineStart.Position + lineEnd.Position) / 2;
            Handles.DrawLine(normalStart, normalStart + _points[p1].Normal);
        }
    }

    protected void OnValidate()
    {
        //if(!Application.isPlaying)
            SantizePoints();
    }

    protected void SantizePoints()
    {
        if (_points.Count == 1)
        {
            LinePoint point1 = _points[0];
            LinePoint point2 = new LinePoint();
            point2.Position = point1.Position + Vector2.right;
            SetNormal(0);
            _points.Add(point2);
        }

        for (int p0 = length - 2, p1 = length - 1, p2 = length; p2 < _points.Count; p0++, p1++, p2++)
        {
            if(p1 < 0) continue;

            Vector2 deltaPosition = Vector2.zero;

            if (p0 >= 0)
            {
                deltaPosition = (_points[p1].Position - _points[p0].Position).normalized;
            }

            if (deltaPosition.magnitude <= .1f)
                deltaPosition = Vector2.right;

            _points[p2].Position = _points[p1].Position + deltaPosition;
            SetNormal(p1);
        }

        length = _points.Count;
    }

    protected void SetNormal(int index)
    {
        LinePoint p1 = _points[index];

        if (index + 1 < _points.Count)
        {
            LinePoint p2 = _points[index + 1];

            p1.Normal = Quaternion.Euler(0, 0, 90) * (p2.Position - p1.Position).normalized;
        }
        else
        {
            p1.Normal = Vector3.up;
        }

        if (Vector2.Dot(p1.Normal, Vector2.up) < 0)
        {
            p1.Normal = -1 * p1.Normal;
        }
    }

    public void Update()
    {
        /*
        if(Application.isPlaying)
            LineCollisionScene.Instance.ShowCount();
        */

        if (previousPosition != transform.position)
        {
            for (int i = 0; i < _points.Count; i++)
            {
                Vector2 deltaPosition = transform.position - previousPosition;
                _points[i].Position = _points[i].Position + deltaPosition;
            }
        }

        previousPosition = transform.position;
    }
}

