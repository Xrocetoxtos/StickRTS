using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverState : BaseFSM
{
    private GatherController gather;
    private MovementController movement;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        gather = unit.gather;
        movement = unit.movement;
        TimeTickSystem.OnTick += TimeTickSystem_OnTick;
    }
    private void TimeTickSystem_OnTick(object sender, TimeTickSystem.OnTickEventArgs e)
    { 
        // resources achterlaten bij building en dan weer naar returnTarget toe.
        unit.player.AddResource(gather.hasResourceType, (int)gather.hasResourceAmount);
        gather.hasResourceAmount = 0;

        unit.animator.SetBool("HasResources", false);
        unit.animator.SetBool("HasArrived", false);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        MovementController[] thisUnit = BigBookBasic.ThisUnitToArray(movement);
        unit.player.playerController.MoveToObject(aIController.returnTarget.gameObject, thisUnit);
        TimeTickSystem.OnTick -= TimeTickSystem_OnTick;

    }
}
