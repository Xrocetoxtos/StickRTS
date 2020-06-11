using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : WorldObject
{
    public ResourceType resourceType;
    public float resourceAmount;
    public float maxResourceAmount = 100;
    public bool available = false;

    public CharacterAnimationState gatherAnimation = CharacterAnimationState.Pickup;

    protected override void Awake()
    {
        base.Awake();
        worldObjectType = ObjectType.Resource;
        resourceAmount = maxResourceAmount;
    }

    public void ExtractResource(float amount, GatherController gather)
    {
        if(resourceAmount<=0)
            DestroyResource(gather);

        if (resourceAmount < amount)
            amount = resourceAmount;

        if(gather.hasResourceType==resourceType)
        {
            if (gather.hasResourceAmount+amount > gather.maxResourceAmount)
                amount = gather.maxResourceAmount - gather.hasResourceAmount;
            if (amount != 0)
            {
                resourceAmount -= amount;
                gather.hasResourceAmount += amount;
                if (resourceAmount <= .1f)
                {
                    gather.hasResourceAmount += resourceAmount;
                    resourceAmount -= resourceAmount;
                }
            }
            else
            {
                //valt niet te extracten.
            }
        }
        else
        {
            if(gather.canExtractResourceType.Contains(resourceType))
            {
                gather.hasResourceAmount = 0;
                gather.hasResourceType = resourceType;
            }
        }
    }

    public float GetResourcePerunage()
    {
        if (maxResourceAmount != 0)
            return resourceAmount / maxResourceAmount;
        else
            return 0;
    }

    protected override void Die()
    {
        base.Die();
        available = true;
    }


    private void DestroyResource(GatherController gatherController)
    {
        //Grid bijwerken, want obstakel valt weg
        //destroy object
        //eventueel character nieuwe instructie geven

    }
}
