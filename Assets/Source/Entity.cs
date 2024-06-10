using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // Has HP
    // Can take damage
    // Can die / be destroyed
    // Can attack
    // Has attack range

    // Connected with Camp (to pass HP, death)

    public delegate void OnDeath(Entity entity);
    public event OnDeath onDeath;

    public string relativeEnemyTag;

    public float approachRange = 0.5f;
}
