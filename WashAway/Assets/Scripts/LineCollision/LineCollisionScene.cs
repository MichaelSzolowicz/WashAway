using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCollisionScene : MonoBehaviour
{
    private static LineCollisionScene _instance;

    public static LineCollisionScene Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<LineCollisionScene>();

                if (_instance != null)
                {
                    return _instance;
                }

                GameObject go = new GameObject();
                go.name = "LineCollisionScene";
                _instance = go.AddComponent<LineCollisionScene>();
            }

            return _instance;
        }
    }

    private void OnEnable()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void OnDestroy()
    {
        if (_instance != null)
        {
            _instance = null;
        }
    }

    private List<ILineColliderInterface> _lineColliders = new List<ILineColliderInterface>();

    public void RegisterLineCollider(ILineColliderInterface lineColliderInterface)
    {
        if (_lineColliders.Contains(lineColliderInterface))
            return;

        _lineColliders.Add(lineColliderInterface);
    }

    public void RemoveLineCollider(ILineColliderInterface lineColliderInterface)
    {
        if (!_lineColliders.Contains(lineColliderInterface))
            return;

        _lineColliders.Remove(lineColliderInterface);
    }

    public bool IntersectLine(Vector3 lineStart, Vector3 lineEnd, out LineIntersectionResult lineIntersectionResult)
    {
        bool result = false;
        lineIntersectionResult = LineIntersectionResult.GetEmpty();

        if (lineStart == lineEnd)
        {
            return false;
        }

        foreach (var lineCollider in _lineColliders)
        {
            LineIntersectionResult testIntersect = LineIntersectionResult.GetEmpty();
            bool validIntersect = lineCollider.IntersectLine(lineStart, lineEnd, out testIntersect);

            if(validIntersect)
            {
                result = true;

                float distance0 = Vector2.Distance(lineStart, testIntersect.intersectPosition);
                float distance1 = Vector2.Distance(lineStart, lineIntersectionResult.intersectPosition);

                float dot0 = Vector2.Dot((lineEnd - lineStart).normalized, testIntersect.surfaceNormal);
                float dot1 = lineIntersectionResult.validIntersection ? Vector2.Dot((lineEnd - lineStart).normalized, lineIntersectionResult.surfaceNormal) : float.PositiveInfinity;

                // Because this is a slow paced game I am assuming any two intersections will be very close to eachother, in which case we take the one that opposes movement more.
                if(dot0 < dot1)
                {
                    lineIntersectionResult = testIntersect;
                }
            }
        }

        return result;
    }

    public void ShowCount()
    {
        Debug.Log(_lineColliders.Count);
    }
}

