using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshPriorityManager : MonoBehaviour
{
    public static int MIN_PRIORITY = 0;
    public static int MAX_PRIORITY = 99;
    public static int SET_MAX_SIZE = 100;
    public static int MAX_LIST_COUNT = 500; // support till 5000 priorities
    private static List<HashSet<int>> hashSetList = new List<HashSet<int>>();

    public static int IssueNewPriority()
    {
        // Halt if we have reached the limit of lists
        if (hashSetList.Count >= MAX_LIST_COUNT)
        {
            Debug.LogError("NavMeshPriorityManager: IssueNewPriority: MAX_LIST_COUNT reached");
            throw new System.Exception("NavMeshPriorityManager: IssueNewPriority: MAX_LIST_COUNT reached");
        }

        HashSet<int> currList = null;
        foreach (HashSet<int> hashSet in hashSetList)
        {
            if (hashSet.Count < SET_MAX_SIZE)
            {
                currList = hashSet;
                break;
            }
        }
        if(currList == null)
        {
            currList = new HashSet<int>();
            hashSetList.Add(currList);
        }

        int priority = genPriority();
        while (currList.Contains(priority))
        {
            priority = genPriority();
        }
        currList.Add(priority);

        return priority;
    }

    public static void ReleasePriority(int priority)
    {
        // Go through all lists but remove only from the first one that contains it
        foreach (HashSet<int> hashSet in hashSetList)
        {
            if (hashSet.Contains(priority))
            {
                hashSet.Remove(priority);
                return;
            }
        }
        // Assert we never reach here
        Debug.LogError("NavMeshPriorityManager: ReleasePriority: priority not found");
    }

    private static int genPriority()
    {
        return Random.Range(MIN_PRIORITY, MAX_PRIORITY);
    }
}
