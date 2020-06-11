using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherState : BaseFSM
{
    private Resource resource;
    private GatherController gather;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        TimeTickSystem.OnTick += TimeTickSystem_OnTick;
        
        gather = unit.gather;
        resource = aIController.actualTarget.GetComponent<Resource>();
        if (resource)
        {
            Vector2 direction = BigBookBasic.GetDirectionVector2(resource.transform.position, unitObject.transform.position);
            characterAnimator.GetAnimationFromVector2(direction.x, direction.y, resource.gatherAnimation);
        }
        else
        {
            animator.SetBool("HasArrived", true);
            animator.SetBool("IsGathering", false);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        if (gather.hasResourceAmount >= gather.maxResourceAmount || (resource.resourceAmount <= 0 && gather.hasResourceAmount > 0))
        {
            anim.SetBool("HasResources", true);
            gather.FindStorage();
        }
        else if (resource.resourceAmount <= 0 && gather.hasResourceAmount <= 0)
            gather.FindNewResource();
    }


    private void TimeTickSystem_OnTick(object sender, TimeTickSystem.OnTickEventArgs e)
    {
        Debug.Log("gather tick");
        //aanroepen van gather mechanic
        resource.ExtractResource(unit.gather.resourceExtractSpeed/5, gather);  //time.deltatime losgelaten hier, want op basis van tick per seconde.

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        aIController.returnTarget = resource;
        TimeTickSystem.OnTick -= TimeTickSystem_OnTick;
    }
}
