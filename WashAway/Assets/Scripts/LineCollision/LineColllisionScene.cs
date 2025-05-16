using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineColllisionScene : MonoBehaviour
{
    private static LineColllisionScene _instance;

    public static LineColllisionScene Instance
    {
        get 
        { 
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<LineColllisionScene>();

                if (_instance != null )
                { 
                    return _instance;
                }

                GameObject go = new GameObject();
                go = Instantiate(go);
                go.name = "LineCollisionScene";
                _instance = go.AddComponent<LineColllisionScene>();
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
        if(_instance != null)
        {
            _instance = null;
        }
    }

    private List<LineCollider> _lineColliders = new List<LineCollider>();

    public void RegisterLineCollider(LineCollider lineCollider)
    {
        if (_lineColliders.Contains(lineCollider))
            return;

        _lineColliders.Add(lineCollider);
    }

    public void RemoveLineCollider(LineCollider lineCollider)
    {
        if(!_lineColliders.Contains(lineCollider))
            return ;

        _lineColliders.Remove(lineCollider);    
    }

    public bool IntersectLine(Vector3 start, Vector3 end, out LineIntersectionResult lineIntersectionResult)
    {
        bool result = false;
        lineIntersectionResult = LineIntersectionResult.GetEmpty();

        if(start == end)
        {
            return false;
        }

        foreach (var lineCollider in _lineColliders)
        {
            if(!lineCollider || !lineCollider.gameObject || !lineCollider.gameObject.activeInHierarchy) continue;

            for (int i = 0, j = 1; j < lineCollider.Length; i++, j++)
            {
                Vector2 colliderStart = lineCollider.GetPointWorldSpace(i);
                Vector2 colliderEnd = lineCollider.GetPointWorldSpace(j);

                Vector3 testIntersect = Vector3.zero;
                bool validIntersection = LineIntersections.IntersectLineLine(start.x, end.x, colliderStart.x, colliderEnd.x, start.y, end.y, colliderStart.y, colliderEnd.y, out testIntersect);

                if(validIntersection)
                {
                    result = true;

                    if (Vector2.Distance(start, testIntersect) < Vector2.Distance(start, lineIntersectionResult.intersectPosition))
                    {
                        lineIntersectionResult.intersectPosition = testIntersect;
                        lineIntersectionResult.intersectDistance = Vector2.Distance(start, testIntersect) / Vector2.Distance(start, end);
                        lineIntersectionResult.surfaceNormal = (Quaternion.Euler(0, 0, 90) * (colliderStart - colliderEnd)).normalized;
                        lineIntersectionResult.validIntersection = result;
                    }
                }
            }
        }

        return result;
    }
}

