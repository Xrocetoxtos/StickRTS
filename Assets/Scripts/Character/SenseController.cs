using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenseController : MonoBehaviour
{
    public float viewRange = 5;
    public Unit unit;

    public bool inDanger = false;

    public float minIdle = 1;   // zo langs blijft een NPC idle staan voor hij iets bedenkt om te doen.
    public float maxIdle = 5;
    private bool foundPath=false;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public Vector2 FindRandomPositon()
    {
        Vector2 position=transform.position;
        foundPath = false;

        while (!foundPath)
        {
            position = new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle * viewRange;
            if(GameManager.instance.grid.PositionIsInGrid(position))
                foundPath = GameManager.instance.grid.NodeFromWorldPoint(position).walkable;
        }
        return position;
    }

    private void CheckPath(Vector3[] wayPoints, bool pathSuccess)
    {
        foundPath = pathSuccess;
    }

    public WorldObject FindStorage(ResourceType resourceType)
    {
        List<Building> buildings = unit.player.GetBuildingsForResourceStorage(resourceType);
        if (buildings.Count == 0)
        {
            Debug.Log(unit.worldObjectName + " kan geen storage vinden voor " + resourceType);
            return null;
        }
        else
            return BigBookBasic.GetNearestWorldObjectInList(transform.position, buildings);
    }     

    public WorldObject FindNewResource(ResourceType resourceType)
    {
        //als een resource leeg is en character heeft geen resources, dan zoeken naar een nieuwe. moet binnen viewRange.
        return BigBookBasic.GetNearestResourceInRange(transform.position, viewRange, resourceType);
    }
}
