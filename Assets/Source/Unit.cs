using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : Entity
{
    // Movement
    Route route;
    NavMeshAgent navMeshAgent;
    int currentWayPointIndex = 0;

    // Agresive
    private SphereCollider sphereCollider;
    public GameObject enemy;

    // Attack
    public int damage = 10;
    public float attackRate = 1.0f;
    Coroutine attackCoroutine = null;

    // Animation
    Animator animator;

    protected void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    public void SetRoute(Route route)
    {
        this.route = route;
    }

    protected void FixedUpdate()
    {
        Move();
    }

    /**
     * TODO: If no more towers there could be a case units never met
     * so lets make them go to each other?
     */
    private void Move()
    {
        // Init must set route
        if (route == null)
        {
            return;
        }

        // To the next waypoint or an enemy
        Vector3 targetDestination = route.wayPointList[currentWayPointIndex].transform.position;
        if(enemy != null)
        {
            targetDestination = enemy.transform.position;
        }

        // Stop at
        float stopDistance = (null == enemy ? approachRange : enemy.GetComponent<Entity>().approachRange);

        // Go
        if (Vector3.Distance(transform.position, targetDestination) > stopDistance)
            navMeshAgent.SetDestination(targetDestination);

        // Look at
        transform.LookAt(targetDestination);

        // Arrived
        bool arrived = Vector3.Distance(transform.position, targetDestination) < stopDistance;

        // Switch to the next waypoint if no enemy
        if (arrived && null == enemy)
        {
            navMeshAgent.enabled = true;
            currentWayPointIndex++;
            if (currentWayPointIndex >= route.wayPointList.Length)
            {
                currentWayPointIndex = 0;
            }
        } else if (arrived && null != enemy)
        {
            navMeshAgent.SetDestination(transform.position); // it's not a proper way to stop
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.isStopped = true;
            navMeshAgent.enabled = false;

            animator.SetBool("isFight", true);
            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(Attack());
            }
        } else if (!arrived && null != enemy)
        {
            // No fight, but take closer
            animator.SetBool("isFight", false);
            if(null != attackCoroutine)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
            navMeshAgent.enabled = true;
        }

        // Get velocity from navMeshAgent
        Vector3 velocity = navMeshAgent.velocity;
        int move = (int)(velocity.magnitude * 100);
        animator.SetInteger("move", move);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag(relativeEnemyTag) && null == enemy)
        {
            enemy = collider.gameObject;
            enemy.GetComponent<Entity>().onDeath += OnEnemyDeath;
        }
    }

    IEnumerator Attack()
    {
        while (true)
        {
            if (enemy != null)
            {
                enemy.GetComponent<Entity>().TakeDamage(damage);
            }
            yield return new WaitForSeconds(attackRate);
        }
    }

    private void OnEnemyDeath(Entity entity)
    {
        entity.onDeath -= OnEnemyDeath;
        enemy = null;
        animator.SetBool("isFight", false);
        // Find the closest way point
        float minDistance = float.MaxValue;
        int minIndex = 0;
        for (int i = 0; i < route.wayPointList.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, route.wayPointList[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                minIndex = i;
            }
        }
        currentWayPointIndex = minIndex;
    }

    private void OnDrawGizmos()
    {
    }

    private void OnDrawGizmosSelected()
    {
        Color color = new Color(0.7f, 0.1f, 0.1f, 0.3f);
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, GetComponent<SphereCollider>().radius);
    }
}
