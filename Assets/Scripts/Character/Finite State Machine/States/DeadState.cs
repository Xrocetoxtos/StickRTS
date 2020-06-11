using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : BaseFSM
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.enabled = false;
        unitObject.TryGetComponent<AIController>(out AIController aIController);
        unitObject.TryGetComponent<MovementController>(out MovementController movementController);
        unitObject.TryGetComponent<GatherController>(out GatherController gatherController);
        unitObject.TryGetComponent<SenseController>(out SenseController senseController);
        unitObject.TryGetComponent<CharacterAnimator>(out CharacterAnimator characterAnimator);

        if (aIController) aIController.enabled = false;
        if (movementController) movementController.enabled = false;
        if (gatherController) gatherController.enabled = false;
        if (senseController) senseController.enabled = false;
        if (characterAnimator) characterAnimator.Disable();
    }
}
