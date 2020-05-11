using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : WorldObject
{
    public List<ResourceType> acceptsResource = new List<ResourceType>();

    protected override void Awake()
    {
        base.Awake();
        worldObjectType = ObjectType.Building;
        if (player != null)
            player.buildings.Add(this);
    }
}
