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

    public bool IntersectLine(Vector3 start, Vector3 end, out Vector3 intersection)
    {
        intersection = Vector3.positiveInfinity;

        return false;
    }
}
