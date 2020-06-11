using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalFindRandomPosition : BaseFSM
{
    private int idleTimer;
    private int ticks;
    MovementController movement;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.gameObject.GetComponent<MovementController>();
        TimeTickSystem.OnTick += TimeTickSystem_OnTick;
        idleTimer = (int)UnityEngine.Random.Range(senseController.minIdle, senseController.maxIdle)*5;
    }

    private void TimeTickSystem_OnTick(object sender, TimeTickSystem.OnTickEventArgs e)
    {
        ticks++;
        if (ticks >= idleTimer)
        {
            anim.SetBool("HasArrived", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        TimeTickSystem.OnTick -= TimeTickSystem_OnTick;
        aIController.targetDummy.position = senseController.FindRandomPositon();
        aIController.targetPosition = aIController.targetDummy;
    }

}
