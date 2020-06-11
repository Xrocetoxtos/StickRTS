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
    }

    protected override void Start()
    {
        base.Start();
        if (player != null)
            player.buildings.Add(this);
    }
}
