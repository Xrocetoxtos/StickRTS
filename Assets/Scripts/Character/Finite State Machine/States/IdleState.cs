using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseFSM
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        characterAnimator.ChangeAnimation(CharacterAnimationState.Idle);
    }
}
