using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : WorldObject
{
    protected override void Awake()
    {
        base.Awake();
        if (player != null)
            player.buildings.Add(this);
    }
}
