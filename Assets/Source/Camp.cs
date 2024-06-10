using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class Camp : MonoBehaviour
{
    // Base player (or AI) logic
    // Spawn units
    // Collets coins
    // Manage towers
    // Manage total health

    // Essentials
    public enum EUnitType
    {
        Swordman,
    }

    public abstract string campTag { get; }

    // Units
    public Route route;
    public Unit swordmanPrefab;

    // Towers
    public Tower towerPrefab;
    public TowerPlace[] towerPlaceList;
    public Dictionary<TowerPlace, Tower> towerDict = new Dictionary<TowerPlace, Tower>();

    // All entities
    public HashSet<Entity> entitySet = new HashSet<Entity>();

    public void Awake()
    {
        tag = campTag;
    }

    public void SpawnUnit(EUnitType unitType)
    {
        if (unitType == EUnitType.Swordman)
        {
            Vector3 dropPosition = route.wayPointList[0].GetComponent<Transform>().position;
            Unit newUnit = Instantiate(swordmanPrefab, dropPosition, Quaternion.identity);
            newUnit.GetComponent<NavMeshAgent>().avoidancePriority = NavMeshPriorityManager.IssueNewPriority();
            newUnit.SetRoute(route);

            newUnit.tag = campTag;
            newUnit.relativeEnemyTag = (campTag == GameManager.PLAYER_TAG ? GameManager.ENEMY_TAG : GameManager.PLAYER_TAG);

            entitySet.Add(newUnit);
            newUnit.onDeath += OnUnitDeath;
        }
    }

    public void BuildTower()
    {
        TowerPlace towerPlace = null;
        foreach (TowerPlace place in towerPlaceList)
        {
            if (towerDict.ContainsKey(place) == false || null == towerDict.GetValueOrDefault(place, null))
            {
                towerPlace = place;
                break;
            }
        }
        if (null == towerPlace)
        {
            return;
        }

        Tower newTower = Instantiate(towerPrefab, towerPlace.transform.position, Quaternion.identity);
        // scale the same scale as the tower place
        newTower.transform.localScale = towerPlace.transform.localScale;
        newTower.tag = campTag;
        newTower.relativeEnemyTag = (campTag == GameManager.PLAYER_TAG ? GameManager.ENEMY_TAG : GameManager.PLAYER_TAG);
        towerDict.Add(towerPlace, newTower);
        newTower.towerPlaceRef = towerPlace;
        entitySet.Add(newTower);
        newTower.onDeath += OnTowerDeath;
    }

    private void OnUnitDeath(Entity entity)
    {
        int priority = entity.GetComponent<NavMeshAgent>().avoidancePriority;
        NavMeshPriorityManager.ReleasePriority(priority);
        entitySet.Remove(entity);
    }

    private void OnTowerDeath(Entity entity)
    {
        entitySet.Remove(entity);
        towerDict.Remove(entity.GetComponent<Tower>().towerPlaceRef);
    }
}
