using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherState : BaseFSM
{
    private Resource resource;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        TimeTickSystem.OnTick += TimeTickSystem_OnTick;

        resource = character.actualTarget.GetComponent<Resource>();
        switch (resource.resourceType)
        {
            case ResourceType.Food:
                break;
            case ResourceType.Wood:
                break;
            case ResourceType.Gold:
                break;
            case ResourceType.Stone:
                Debug.Log("mine");
                characterAnimator.ChangeAnimation(CharacterAnimationState.Mine);
                break;
            case ResourceType.None:
                break;
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        if (character.hasResourceAmount >= character.maxResourceAmount || (resource.resourceAmount <= 0 && character.hasResourceAmount > 0))
        {
            anim.SetBool("HasResources", true);
            character.FindStorage();
        }
        else if (resource.resourceAmount <= 0 && character.hasResourceAmount <= 0)
            character.FindNewResource();
    }


    private void TimeTickSystem_OnTick(object sender, TimeTickSystem.OnTickEventArgs e)
    {
        Debug.Log("gather tick");
        //aanroepen van gather mechanic
        resource.ExtractResource(character.resourceExtractSpeed * Time.deltaTime * 5, character);

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        character.returnTarget = resource;
        TimeTickSystem.OnTick -= TimeTickSystem_OnTick;
    }
}
