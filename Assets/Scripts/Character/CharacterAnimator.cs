using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private CharacterAnimation[] characterAnimations;
    [SerializeField] private CharacterAnimation currentCharacterAnimation;
    [SerializeField] private int frameNumber;
    [SerializeField] private float timer = 0;
    [SerializeField] private float timerMax = .2f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentCharacterAnimation = GetCharacterAnimation("Idle");
        SetUpAnimation();
    }

    public void ChangeAnimation(string animationName)
    {
        if(currentCharacterAnimation.characterAnimationName != animationName)
        {
            currentCharacterAnimation = GetCharacterAnimation(animationName);
            SetUpAnimation();
        }
    }

    public void ChangeAnimation(CharacterAnimationState state)
    {
        if (currentCharacterAnimation.characterAnimationState != state)
        {
            currentCharacterAnimation = GetCharacterAnimation(state);
            SetUpAnimation();
        }
    }

    private void SetUpAnimation()
    {
        frameNumber = 0;
        spriteRenderer.flipX = currentCharacterAnimation.flipX;
        timer = 0;
        timerMax = currentCharacterAnimation.frameRate;
    }

    private void Update()
    {
        if (currentCharacterAnimation.animation.Length == 0)
            return;

        if (frameNumber >= currentCharacterAnimation.animation.Length)
            frameNumber = 0;

        if (timer>timerMax)
        {
            timer -= timerMax;
            frameNumber++;
            if (frameNumber >= currentCharacterAnimation.animation.Length)
                frameNumber = 0;
            spriteRenderer.sprite = currentCharacterAnimation.animation[frameNumber];
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    public void GetAnimationFromVector2(float x, float y)
    {
        CharacterAnimationState state = CharacterAnimationState.WalkLR;
        if (x == 0 && y != 0)
        {
            if ((y > 0))
                ChangeAnimation(CharacterAnimationState.WalkDU);
            else
                ChangeAnimation(CharacterAnimationState.WalkUD);
            return;
        }

        if (x < 0)
        {
            x *= -1;
            state = CharacterAnimationState.WalkRL;
        }

        if (y > x || (y < 0 && y + x < 0))
        {
            if ((y > 0))
                ChangeAnimation(CharacterAnimationState.WalkDU);
            else
                ChangeAnimation(CharacterAnimationState.WalkUD);
        }
        else
            ChangeAnimation(state);
    }

    private CharacterAnimation GetCharacterAnimation(string name)
    {
        for (int i = 0; i < characterAnimations.Length; i++)
        {
            if (characterAnimations[i].characterAnimationName == name)
                return characterAnimations[i];
        }
        return characterAnimations[0];
    }
    private CharacterAnimation GetCharacterAnimation(CharacterAnimationState state)
    {
        for (int i = 0; i < characterAnimations.Length; i++)
        {
            if (characterAnimations[i].characterAnimationState == state)
                return characterAnimations[i];
        }
        return characterAnimations[0];
    }
}

[System.Serializable]
public struct CharacterAnimation
{
    public string characterAnimationName;
    public CharacterAnimationState characterAnimationState;
    public float frameRate;
    public Sprite[] animation;
    public bool flipX;
}
