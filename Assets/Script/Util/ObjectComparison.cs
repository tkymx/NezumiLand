using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectComparison
{

    public static Vector3 Direction(Vector3 to, Vector3 from)
    {
        Vector3 direction = to - from;
        direction.y = 0;
        return direction.normalized;
    }

        public static float Distance(Vector3 a, Vector3 b)
    {
        Vector3 direction = a - b;
        direction.y = 0;
        return direction.magnitude;
    }
}