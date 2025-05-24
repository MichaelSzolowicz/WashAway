using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public static class LineIntersections
{
    private static bool HasIntersection(float x0, float x1, float x2, float x3, float y0, float y1, float y2, float y3)
    {
        return false;
    }

    private static float Orientation(float x0, float x1, float x2, float y0, float y1, float y2)
    {
        return (y1 - y0) * (x2 - x1) - (x1 - x0) * (y2 - y1);
    }

    public static bool IntersectLineLine2(float x0, float x1, float x2, float x3, float y0, float y1, float y2, float y3, out Vector3 intersectPoisition)
    {
        intersectPoisition = Vector3.positiveInfinity;

        float orientation203 = Orientation(x2, x0, x3, y2, y0, y3);
        float orientation213 = Orientation(x2, x1, x3, y2, y1, y3);

        float tolerance = .001f;
        if ((Mathf.Abs(orientation203) <= tolerance && Mathf.Abs(orientation213) > tolerance) ||
            (Mathf.Abs(orientation213) <= tolerance && Mathf.Abs(orientation203) > tolerance))
        {
            float u = -((x0 - x1) * (y0 - y2) - (y0 - y1) * (x0 - x2)) / ((x0 - x1) * (y2 - y3) - (y0 - y1) * (x2 - x3));

            tolerance = .001f;
            if (u <= -tolerance || u >= tolerance + 1)
            {
                //Debug.Log(u + " FALSE");
                return false;
            }

            intersectPoisition.x = x2 + u * (x3 - x2);
            intersectPoisition.y = y2 + u * (y3 - y2);

            return true;
        }

        float orientation012 = Orientation(x0, x1, x2, y0, y1, y2);
        float orientation013 = Orientation(x0, x1, x3, y0, y1, y3);
        float orientation231 = Orientation(x2, x3, x1, y2, y3, y1);
        float orientation230 = Orientation(x2, x3, x0, y2, y3, y0);

        if (orientation012 * orientation013 < 0 && orientation231 * orientation230 < 0)
        {
            float t = ((x0 - x2) * (y2 - y3) - (y0 - y2) * (x2 - x3)) / ((x0 - x1) * (y2 - y3) - (y0 - y1) * (x2 - x3));

            intersectPoisition.x = x0 + t * (x1 - x0);
            intersectPoisition.y = y0 + t * (y1 - y0);

            return true;
        }

        //Debug.Log("FALSE");
        return false;
    }

    public static bool IntersectLineLine(float x0, float x1, float x2, float x3, float y0, float y1, float y2, float y3, out Vector3 intersectPosition)
    {
        intersectPosition = Vector3.positiveInfinity;

        float m = (y1 - y0) / (x1 - x0);
        float c = y0 - (m * x0);

        float n = (y3 - y2) / (x3 - x2);
        float b = y2 - n * x2;

        Vector3 testIntersect = Vector3.zero;

        if(Mathf.Abs(m - n) <= .001f)
        {
            return false;
        }
        if (Mathf.Abs(x1 - x0) <= .001f)
        {
            testIntersect.x = x0 + (x1 - x0) / 2;
            testIntersect.y = n * x0 + b;
        }
        else
        {
            testIntersect.x = (b - c) / (m - n);
            testIntersect.y = m * testIntersect.x + c;
        }

        if (testIntersect.x >= Mathf.Min(x0, x1) && testIntersect.x <= Mathf.Max(x0, x1) &&
            testIntersect.x >= Mathf.Min(x2, x3) && testIntersect.x <= Mathf.Max(x2, x3) &&
            testIntersect.y >= Mathf.Min(y0, y1) && testIntersect.y <= Mathf.Max(y0, y1) &&
            testIntersect.y >= Mathf.Min(y2, y3) && testIntersect.y <= Mathf.Max(y2, y3))
        {
            intersectPosition = testIntersect;

            //DrawDebugIntersect(x0, x1, x2, x3, m, n, c, b, testIntersect, true);

            return true;
        }

        //DrawDebugIntersect(x0, x1, x2, x3, m, n , c, b, testIntersect, false);

        return false;
    }

    public static void DrawDebugIntersect(float x0, float x1, float x2, float x3, float m, float n, float c, float b, Vector3 intersect, bool validIntersect)
    {
        Vector3 start1 = new Vector3(x0, m * x0 + c);
        Vector3 end1 = new Vector3(x1, m * x1 + c);
        Vector3 start2 = new Vector3(x2, n * x2 + b);
        Vector3 end2 = new Vector3(x3, n * x3 + b);

        Vector3 normal = Quaternion.Euler(0, 0, 90) * (start2 - end2).normalized;
        Color normalColor = validIntersect ? Color.green : Color.red;

        Debug.DrawLine(start1, end1, Color.white);
        Debug.DrawLine(start2, end2, Color.magenta);
        Debug.DrawLine(intersect, intersect + normal, normalColor);

    }
};
