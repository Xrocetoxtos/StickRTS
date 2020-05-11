using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerName;
    public PlayerSelection playerSelection;
    public PlayerController playerController;
    public bool isHuman = false;

    private int wood;
    private int food;
    private int gold;
    private int stone;

    public List<WorldObjectAction> completedActions = new List<WorldObjectAction>();

    public List<WorldObject> characters = new List<WorldObject>();
    public List<WorldObject> buildings = new List<WorldObject>();

    public event EventHandler OnResourcesChanged;


    private void Awake()
    {
        playerSelection = GetComponent<PlayerSelection>();
        playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        if (OnResourcesChanged != null) OnResourcesChanged(this, EventArgs.Empty);
    }


    public bool ActionAvailable(WorldObjectAction action)
    {
        int count = action.prerequisiteActions.Count;
        if (count==0)
            return true;

        int number = 0;
        foreach(WorldObjectAction pre in action.prerequisiteActions)
        {
            if (completedActions.Contains(pre))
                number++;
        }
        return number == count;
    }

    public int GetResource(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Food:
                return food;
            case ResourceType.Wood:
                return wood;
            case ResourceType.Gold:
                return gold;
            case ResourceType.Stone:
                return stone;
            default:
                return 0;
        }
    }
    public bool AddResource(ResourceType resourceType, int amount)
    {
        if (amount < 0)
            return false;
        switch (resourceType)
        {
            case ResourceType.Food:
                food+=amount;
                break;
            case ResourceType.Wood:
                wood+=amount;
                break;
            case ResourceType.Gold:
                gold += amount;
                break;
            case ResourceType.Stone:
                stone += amount;
                break;
            default:
                return false;
        }
        if (OnResourcesChanged != null) OnResourcesChanged(this, EventArgs.Empty);
        return true;
    }
    public bool RemoveResource(ResourceType resourceType, int amount)
    {
        if (amount < 0)
            return false;
        switch (resourceType)
        {
            case ResourceType.Food:
                if (food >= amount)
                {
                    food -= amount;
                    break;
                }
                else
                    return false;
            case ResourceType.Wood:
                if (wood >= amount)
                {
                    wood -= amount;
                    break;
                }
                else
                    return false;
            case ResourceType.Gold:
                if (gold >= amount)
                {
                    gold -= amount;
                    break;
                }
                else
                    return false;
            case ResourceType.Stone:
                if (stone >= amount)
                {
                    stone -= amount;
                    break;
                }
                else
                    return false;
            default:
                return false;
        }
        if (OnResourcesChanged != null) OnResourcesChanged(this, EventArgs.Empty);
        return true;
    }

    public List<Building> GetBuildingsForResourceStorage(ResourceType resourceType=ResourceType.None)
    {
        if (resourceType == ResourceType.None)
            return new List<Building>();

        List<Building> buildingList = new List<Building>();
        foreach(Building building in buildings)
        {
            if (building.acceptsResource.Contains(resourceType))
                buildingList.Add(building);
        }
        return buildingList;
    }
}
