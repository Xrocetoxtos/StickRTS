using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFSM : StateMachineBehaviour
{
    public GameObject characterObject;
    public Character character;
    protected CharacterAnimator characterAnimator;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterObject = animator.gameObject;
        character = characterObject.GetComponent<Character>();
        characterAnimator = characterObject.GetComponent<CharacterAnimator>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

}
