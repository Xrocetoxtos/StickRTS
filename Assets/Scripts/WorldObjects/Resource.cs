using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : WorldObject
{
    public ResourceType resourceType;
    public int resourceAmount=100;

    protected override void Awake()
    {
        base.Awake();
    }

    public void ExtractResource(int amount, Character character)
    {
        if(resourceAmount<=0)
            DestroyResource(character);

        if (resourceAmount < amount)
            amount = resourceAmount;

        if(character.hasResourceType==resourceType)
        {
            if (character.hasResourceAmount+amount > character.maxResourceAmount)
                amount = character.maxResourceAmount - character.hasResourceAmount;

            if (amount != 0)
            {
                resourceAmount -= amount;
                character.hasResourceAmount += amount;
            }
            else
            {
                //valt niet te extracten.
            }
        }
    }

    private void DestroyResource(Character character)
    {
        //Grid bijwerken, want obstakel valt weg
        //destroy object
        //eventueel character nieuwe instructie geven

    }
}
