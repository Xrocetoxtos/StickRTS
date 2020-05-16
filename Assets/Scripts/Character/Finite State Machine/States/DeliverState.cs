using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverState : BaseFSM
{
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    base.OnStateEnter(animator, stateInfo, layerIndex);

    //}

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        // resources achterlaten bij building en dan weer naar returnTarget toe.
        character.player.AddResource(character.hasResourceType, (int)character.hasResourceAmount);
        character.hasResourceAmount = 0;

        animator.SetBool("HasResources", false);
        animator.SetBool("HasArrived", false);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        Character[] thisCharacter = new Character[1];
        thisCharacter[0] = character;
        character.player.playerController.MoveToObject(character.returnTarget.gameObject, thisCharacter);

    }
}
