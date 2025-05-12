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

    public bool IntersectLine(Vector3 start, Vector3 end, out Vector3 outIntersect)
    {
        bool foundIntesrection = false;
        outIntersect = Vector3.positiveInfinity;

        foreach (var lineCollider in _lineColliders)
        {
            for (int i = 0, j = 1; j < lineCollider.Length; i++, j++)
            {
                float pm = (end.y - start.y) / (end.x - start.x);
                float pc = start.y - pm * start.x;

                Vector2 c0 = lineCollider.GetPointWorldSpace(i);
                Vector2 c1 = lineCollider.GetPointWorldSpace(j);

                float cm = (c1.y - c0.y) / (c1.x - c0.x);
                float cc = c0.y - cm * c0.x;

                Vector3 intersect = new Vector3();

                //if (pm == cm) continue;

                intersect.x = (pc - cc) / (cm - pm);
                intersect.y = cm * ((pc - cc) / (cm - pm)) + cc;

                if(intersect.x >= Mathf.Min(c0.x, c1.x) && intersect.x <= Mathf.Max(c0.x, c1.x) &&
                    intersect.y >= Mathf.Min(c0.y, c1.y) && intersect.y <= Mathf.Max(c0.y, c1.y))
                {
                    foundIntesrection = true;
                    if (Vector2.Distance(start, intersect) < Vector2.Distance(start, outIntersect))
                    {
                        outIntersect = intersect;
                    }
                }

                Debug.DrawLine(start, end + (end - start).normalized * 1, Color.green);
                Debug.DrawLine(c0, c1, Color.red);
                Debug.DrawLine(intersect, intersect + (end - start).normalized);
            }
        }

        return foundIntesrection;
    }
}
