using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class LineIntersections
{
    public static bool IntersectLineLine(float x0, float x1, float x2, float x3, float y0, float y1, float y2, float y3, out Vector3 intersect)
    {
        intersect = Vector3.positiveInfinity;

        float m = (y1 - y0) / (x1 - x0);
        float c = y0 - (m * x0);

        float n = (y3 - y2) / (x3 - x2);
        float b = y2 - n * x2;

        Vector3 testIntersect = Vector3.zero;

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
            intersect = testIntersect;

            DrawDebugIntersect(x0, x1, x2, x3, m, n, c, b, intersect, true);

            return true;
        }

        DrawDebugIntersect(x0, x1, x2, x3, m, n , c, b, testIntersect, false);

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
