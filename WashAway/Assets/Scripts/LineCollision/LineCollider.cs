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
    protected int length = 0;

    [SerializeField] protected float rotation = 0;

    [SerializeField] protected float width = 0;

    [SerializeField] protected bool visibleInGame = true;
    [SerializeField] protected bool visibleInEditor = true;

    [SerializeField] protected Color defaultColor = Color.white;
    [SerializeField] protected Color selectedColor = Color.yellow;
    [SerializeField] protected float pointSize = .01f;
    [SerializeField] protected float lineWidth = 1;

    protected bool isSelected = false;

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
        SantizePoints();
        if (Application.isPlaying)
            LineCollisionScene.Instance.RegisterLineCollider(this);
    }

    public Vector3 GetPointWorldSpace(int index)
    {
        if(index < 0 || index >= _points.Count)
        {
            throw new IndexOutOfRangeException("Index out of range.");
        }

        Vector3 rotatedPoint = new Vector3();

        float rotation = Mathf.Deg2Rad * this.rotation;
        rotatedPoint.x = _points[index].x * Mathf.Cos(rotation) - _points[index].y * Mathf.Sin(rotation);
        rotatedPoint.y = _points[index].x * Mathf.Sin(rotation) + _points[index].y * Mathf.Cos(rotation);

        return transform.position + rotatedPoint;
    }

    public void SetPointWorldPosition(int index, Vector3 worldPosition)
    {
        if (index < 0 || index >= _points.Count)
        {
            throw new IndexOutOfRangeException("Index out of range.");
        }

        Vector3 localPosition = worldPosition - transform.position;

        Vector3 rotatedPoint = new Vector3();

        float rotation = Mathf.Deg2Rad * this.rotation;
        rotatedPoint.x = (localPosition.x * Mathf.Cos(-rotation) - localPosition.y * Mathf.Sin(-rotation));
        rotatedPoint.y = (localPosition.y * Mathf.Cos(-rotation) + localPosition.x * Mathf.Sin(-rotation));

        _points[index].Position = rotatedPoint;


        FaceNormalUp(_points[index]);
        if(index - 1 > 0)
        {
            FaceNormalUp(_points[index - 1]);
        }
    }

    public LinePoint GetLinePointInternal(int index)
    {
        if (index < 0 || index >= _points.Count)
        {
            throw new IndexOutOfRangeException("Index out of range.");
        }

        return _points[index];
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
            Vector3 position = GetPointWorldSpace(i);

            Gizmos.DrawIcon(position, "point", false, useColor);
        }

        for (int p1 = 0, p2 = 1; p2 < Length; p1++, p2++)
        {
            Vector3 lineStart = GetPointWorldSpace(p1);
            Vector3 lineEnd = GetPointWorldSpace(p2);

            Handles.DrawLine(lineStart, lineEnd, lineWidth);
        }

        Handles.color = Color.white;
        for (int p1 = 0, p2 = 1; p2 < Length; p1++, p2++)
        {
            Vector3 lineStart = GetPointWorldSpace(p1);
            Vector3 lineEnd = GetPointWorldSpace(p2);

            Vector3 normalStart = (lineStart + lineEnd) / 2;
            Handles.DrawLine(normalStart, normalStart + _points[p1].Normal);
        }
    }

    protected void OnValidate()
    {
        SantizePoints();
    }

    protected void SantizePoints()
    {
        if (_points.Count == 1)
        {
            LinePoint point1 = _points[0];
            LinePoint point2 = new LinePoint();
            _points.Add(point2);
        }

        for (int i = 0, j = 1; j < _points.Count; i++, j++)
        {
            LinePoint p1 = _points[i];
            LinePoint p2 = _points[j];

            p1.Next = p2;

            FaceNormalUp(p1);
        }

        length = _points.Count;
    }

    protected void FaceNormalUp(LinePoint point)
    {
        if (Vector2.Dot(point.GetNormalUnflipped(), Vector2.up) < 0)
        {
            point.FlipNormal = true;
        }
        else
        {
            point.FlipNormal = false;
        }
    }

    /*
    public void Update()
    {
        if(Application.isPlaying)
            LineCollisionScene.Instance.ShowCount();
    }
    */
}
