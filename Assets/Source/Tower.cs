using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Entity_old
{
    // Cannot move
    // Took a placeholder and event-linked to it
    public TowerPlace towerPlaceRef;


    protected override void Start()
    {
        base.Start();
        approachRange = 2.5f;
        // scale approach range by the bigger scale X/Z
        approachRange *= Mathf.Max(transform.localScale.x, transform.localScale.z);
    }

    private void OnDrawGizmosSelected()
    {
        Color color = new Color(0.7f, 0.1f, 0.1f, 0.3f);
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, GetComponent<SphereCollider>().radius);
    }

    private void OnTriggerEnter(Collider colliser)
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, approachRange);
    }
}
