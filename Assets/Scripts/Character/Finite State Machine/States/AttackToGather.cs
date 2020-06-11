using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackToGather : BaseFSM
{
    private Resource targetResource;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        TimeTickSystem.OnTick += TimeTickSystem_OnTick;

        targetResource = aIController.actualTarget.GetComponent<Resource>();

        Vector2 direction = BigBookBasic.GetDirectionVector2(targetResource.transform.position, unitObject.transform.position);
        characterAnimator.GetAnimationFromVector2(direction.x, direction.y, targetResource.killAnimation);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
    }

    private void TimeTickSystem_OnTick(object sender, TimeTickSystem.OnTickEventArgs e)
    {
        Debug.Log("attack tick");
        //aanroepen van gather mechanic
        targetResource.ChangeHealth(-fightController.meleeAtack);
        if(targetResource.available)
            anim.SetBool("IsAttacking", false);

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        TimeTickSystem.OnTick -= TimeTickSystem_OnTick;
    }

}
