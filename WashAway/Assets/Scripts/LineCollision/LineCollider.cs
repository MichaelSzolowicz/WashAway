using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class LineCollider : MonoBehaviour, ILineColliderInterface
{
    [SerializeField] protected List<LinePoint> _points = new List<LinePoint>();
    [HideInInspector] [SerializeField] protected int length = 0;

    [SerializeField] protected float rotation = 0;

    [SerializeField] protected float width = 0;

    [SerializeField] protected bool visibleInGame = true;
    [SerializeField] protected bool visibleInEditor = true;

    [SerializeField] public Color defaultColor = Color.white;
    [SerializeField] public Color selectedColor = Color.yellow;
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

    public LinePoint GetPoint(int index)
    {
        return _points[index];
    }

    public void SetPointWorldPosition(int index, Vector3 worldPosition)
    {
        _points[index].position = worldPosition;

        SetNormal(index);
        if (index - 1 >= 0)
            SetNormal(index - 1);
    }

    public bool IntersectLine(Vector3 lineStart, Vector3 lineEnd, out LineIntersectionResult intersectionResult)
    {
        bool result = false;
        intersectionResult = LineIntersectionResult.GetEmpty();

        for (int i = 0, j = 1; j < length; i++, j++)
        {
            LinePoint colliderStart = _points[i];
            LinePoint colliderEnd = _points[j];

            Vector3 testIntersect = Vector3.zero;
            bool validIntersection = LineIntersections.IntersectLineLine2(lineStart.x, lineEnd.x, colliderStart.x, colliderEnd.x, lineStart.y, lineEnd.y, colliderStart.y, colliderEnd.y, out testIntersect);

            if (validIntersection)
            {
                result = true;

                if (Vector2.Distance(lineStart, testIntersect) < Vector2.Distance(lineStart, intersectionResult.intersectPosition))
                {
                    intersectionResult.intersectPosition = testIntersect;
                    intersectionResult.intersectDistance = Vector2.Distance(lineStart, testIntersect) / Vector2.Distance(lineStart, lineEnd);
                    intersectionResult.surfaceNormal = colliderStart.normal;
                    intersectionResult.validIntersection = result;
                }
            }
        }

        return result;
    }

    private void OnEnable()
    {
        if (Application.isPlaying)
            LineCollisionScene.Instance.RegisterLineCollider(this);
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

            Gizmos.DrawIcon(point.position, "point", false, useColor);
        }

        for (int p1 = 0, p2 = 1; p2 < Length; p1++, p2++)
        {
            LinePoint lineStart = GetPoint(p1);
            LinePoint lineEnd = GetPoint(p2);

            Handles.DrawLine(lineStart.position, lineEnd.position, lineWidth);
        }

        Handles.color = Color.white;
        for (int p1 = 0, p2 = 1; p2 < Length; p1++, p2++)
        {
            LinePoint lineStart = GetPoint(p1);
            LinePoint lineEnd = GetPoint(p2);

            Vector2 normalStart = (lineStart.position + lineEnd.position) / 2;
            Handles.DrawLine(normalStart, normalStart + _points[p1].normal);
        }
    }

    protected void OnValidate()
    {
        SantizePoints();
    }

    protected void SantizePoints()
    {
        if (length == 0 && _points.Count > 0)
        {
            _points[0].position = transform.position;
        }


        for (int p0 = length - 2, p1 = length - 1, p2 = length; p2 < _points.Count; p0++, p1++, p2++)
        {
            if(p1 < 0) continue;

            Vector2 deltaPosition = Vector2.zero;

            if (p0 >= 0)
            {
                deltaPosition = (_points[p1].position - _points[p0].position).normalized;
            }

            if (deltaPosition.magnitude <= .1f)
                deltaPosition = Vector2.right;

            _points[p2].position = _points[p1].position + deltaPosition;
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

            p1.normal = Quaternion.Euler(0, 0, 90) * (p2.position - p1.position).normalized;
        }
        else
        {
            p1.normal = Vector3.up;
        }

        if (Vector2.Dot(p1.normal, Vector2.up) < 0)
        {
            p1.normal = -1 * p1.normal;
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
                _points[i].position = _points[i].position + deltaPosition;
            }
        }

        previousPosition = transform.position;
    }
}

