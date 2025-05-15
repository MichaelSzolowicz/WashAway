using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class LineCollider : MonoBehaviour
{
    [SerializeField] protected List<Vector2> _points = new List<Vector2>();

    [SerializeField] protected float rotation = 0;

    [SerializeField] protected float width = 0;

    [SerializeField] protected bool visibleInGame = true;
    [SerializeField] protected bool visibleInEditor = true;

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

        _points[index] = rotatedPoint;
    }

    protected void OnDrawGizmos()
    {
        if (!visibleInEditor && !visibleInGame) return;
        else if (SceneView.currentDrawingSceneView == null && !visibleInGame) return;
        else if(SceneView.currentDrawingSceneView != null && !visibleInEditor) return;

        Handles.color = Color.blue;
        for (int i = 0; i < Length; i++)
        {
            Vector3 position = GetPointWorldSpace(i);

            Handles.DrawSolidDisc(position, -Vector3.forward, HandleUtility.GetHandleSize(position) * .1f);
        }

        for (int p1 = 0, p2 = 1; p2 < Length; p1++, p2++)
        {
            Handles.DrawLine(GetPointWorldSpace(p1), GetPointWorldSpace(p2), width);
        }
    }
}

