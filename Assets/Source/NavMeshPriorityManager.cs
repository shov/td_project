using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshPriorityManager : MonoBehaviour
{
    private static HashSet<int> registered = new HashSet<int>();

    public static int IssueNewPriority()
    {
        int priority = Random.Range(0, int.MaxValue);
        while (registered.Contains(priority))
        {
            priority = Random.Range(0, int.MaxValue);
        }
        registered.Add(priority);
        return priority;
    }

    public static void ReleasePriority(int priority)
    {
        registered.Remove(priority);
    }
}
