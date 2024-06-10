using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [SerializeField] Color gizmoColor = Color.blue;

    public void SetGizmoColor(Color color)
    {
        gizmoColor = color;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, 0.4f);
    }
}
