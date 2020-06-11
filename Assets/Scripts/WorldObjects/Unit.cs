using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : WorldObject
{
    public GatherController gather;
    public Animator animator;

    protected override void Awake()
    {
        base.Awake();
        worldObjectType = ObjectType.Character;
        gather = GetComponent<GatherController>();
        if (player != null)
            player.characters.Add(this);

        animator = GetComponent<Animator>();
        animator.SetBool("HasArrived", true);
    }
}
