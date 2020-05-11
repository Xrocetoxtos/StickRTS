using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherState : BaseFSM
{
    private Resource resource;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
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
                characterAnimator.ChangeAnimation(CharacterAnimationState.Mine);
                break;
            case ResourceType.None:
                break;
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        //aanroepen van gather mechanic
        resource.ExtractResource(character.resourceExtractSpeed * Time.deltaTime, character);
        if (character.hasResourceAmount >= character.maxResourceAmount || (resource.resourceAmount <= 0 && character.hasResourceAmount > 0))
        {
            character.FindStorage();
        }
        else if (resource.resourceAmount <= 0 && character.hasResourceAmount <= 0)
            character.FindNewResource();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

    }
}
