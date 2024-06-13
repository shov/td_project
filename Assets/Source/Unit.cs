using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Unit : Entity
{
    // Movement
    Route route;
    int currentWayPointIndex = 0;
    Vector3 currDestination;
    Coroutine moveCorutine;
    Vector3[] pathPointList;
    NavMeshPath navMeshPath;

    // Attack
    private SphereCollider sphereCollider;
    public GameObject enemy;

    // Fight
    public int damage = 10;
    public float attackRate = 1.0f;
    Coroutine attackCoroutine = null;

    // Animation
    Animator animator;

    protected void Awake()
    {

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
        if (enemy != null)
        {
            targetDestination = enemy.transform.position;
        }
        else { }


        // Stop at
        float stopDistance = (null == enemy ? approachRange : enemy.GetComponent<Entity>().approachRange);


        // Go
        if (currDestination != targetDestination)
        {
            if (null != moveCorutine)
            {
                StopCoroutine(moveCorutine);
            }
            MoveTo(targetDestination);
        }

        // Look at
        transform.LookAt(targetDestination);

        // Arrived
        bool arrived = Vector3.Distance(transform.position, targetDestination) < stopDistance;

        // Switch to the next waypoint if no enemy
        if (arrived && null == enemy)
        {
            currentWayPointIndex++;
            if (currentWayPointIndex >= route.wayPointList.Length)
            {
                currentWayPointIndex = 0;
            }
        }
        else if (arrived && null != enemy)
        {
            animator.SetBool("isFight", true);
            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(Fight());
            }
        }
        else if (!arrived && null != enemy)
        {
            // No fight, but take closer
            animator.SetBool("isFight", false);
            if (null != attackCoroutine)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }

    }

    private void MoveTo(Vector3 destination)
    {
        
    }

    private IEnumerator MoveToRoutine()
    {
        yield return null;
    }


    // TODO colliders? 
    private void OnTriggerEnter(Collider other)
    {
        Attak(other);
    }

    private void OnTriggerStay(Collider other)
    {
        Attak(other);
    }

    protected void Attak(Collider other)
    {
        if (other.gameObject.CompareTag(relativeEnemyTag) && null == enemy)
        {
            if (
                null == enemy
                ||
                (Vector3.Distance(transform.position, enemy.transform.position)
                    > Vector3.Distance(transform.position, other.GetComponent<Transform>().position))
            )
            {
                if (enemy)
                {
                    // Unsubscribe from the previous enemy first
                    enemy.GetComponent<Entity>().onDeath -= OnEnemyDeath;
                }
                enemy = other.gameObject;
                enemy.GetComponent<Entity>().onDeath += OnEnemyDeath;
            }
        }
    }

    IEnumerator Fight()
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
        if (null != attackCoroutine)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
        if (gameObject == null)
        {
            return;
        }

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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, approachRange);
    }

    private void OnDrawGizmosSelected()
    {
        Color color = new Color(0.7f, 0.1f, 0.1f, 0.3f);
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, GetComponent<SphereCollider>().radius);
    }
}
