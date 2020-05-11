using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    public readonly Vector3[] lookPoints;
    public readonly int finishLineIndex;
    public readonly int slowDownIndex;

    public Path(Vector3[] waypoints, Vector2 startPosition, float stoppingDistance)
    {
        lookPoints = waypoints;

        Vector2 previousPoint = startPosition;
        for (int i = 0; i < lookPoints.Length; i++)
        {
            Vector2 currentPoint = lookPoints[i];
            Vector2 directionToCurrentPoint = (currentPoint - previousPoint).normalized;
        }

        float distanceFromEndPoint = 0;
        for (int i = lookPoints.Length-1; i > 0; i--) 
        {
            distanceFromEndPoint += Vector2.Distance(lookPoints[i], lookPoints[i - 1]);
            if (distanceFromEndPoint > stoppingDistance)
            {
                slowDownIndex = i;
                break;
            }
        }
    }
}
