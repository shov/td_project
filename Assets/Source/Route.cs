using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    // Unit Walking Route
    // Waypoints
    public WayPoint[] wayPointList;
    public Color gizmoColor = Color.blue;

    private void Awake()
    {
    }

    private void OnDrawGizmos()
    {
        if (wayPointList.Length > 1)
        {
            // Draw lines between waypoints
            for (int i = 0; i < wayPointList.Length; i++)
            {
                if (i + 1 < wayPointList.Length)
                {
                    Debug.DrawLine(wayPointList[i].transform.position, wayPointList[i + 1].transform.position, gizmoColor);
                }
            }
            if(wayPointList.Length > 2)
            {
                Debug.DrawLine(wayPointList[wayPointList.Length - 1].transform.position, wayPointList[0].transform.position, gizmoColor);
            }
        }
    }
}
