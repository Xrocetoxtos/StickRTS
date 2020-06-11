using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFSM : StateMachineBehaviour
{
    public GameObject unitObject;
    public Unit unit;
    protected CharacterAnimator characterAnimator;
    protected AIController aIController;
    protected SenseController senseController;
    protected FightController fightController;
    protected Animator anim;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        anim = animator;
        unitObject = animator.gameObject;
        unit = unitObject.GetComponent<Unit>();
        characterAnimator = unitObject.GetComponent<CharacterAnimator>();
        senseController = unitObject.GetComponent<SenseController>();
        aIController = unitObject.GetComponent<AIController>();
        fightController = unitObject.GetComponent<FightController>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

}
