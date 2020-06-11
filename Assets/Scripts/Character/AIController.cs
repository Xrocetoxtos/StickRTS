using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private CharacterAnimator characterAnimator;
    private Unit unit;
    private SenseController sense;

    private GatherController gather;
    private MovementController movement;

    public Transform targetPosition;
    public WorldObject actualTarget;
    public WorldObject returnTarget;
    public Transform targetDummy;

    public Animator animator;

    private void Awake()
    {
        characterAnimator = GetComponent<CharacterAnimator>();
        unit = GetComponent<Unit>();
        sense = GetComponent<SenseController>();

        movement = GetComponent<MovementController>();
        gather = GetComponent<GatherController>();

        animator = GetComponent<Animator>();
    }

    // ============================ ANIMATIONS ===================================

    public void AnimateDirection(Vector2 direction, CharacterAnimationState state)
    {
        if (characterAnimator)
            characterAnimator.GetAnimationFromVector2(direction.x, direction.y, state);
    }

    public void AnimateSpecific(CharacterAnimationState stickFigureAnimation)
    {
        if (characterAnimator)
            characterAnimator.ChangeAnimation(stickFigureAnimation);
    }
}
