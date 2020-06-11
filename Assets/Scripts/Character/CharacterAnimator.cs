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
    [SerializeField] private int currentTick;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentCharacterAnimation = GetCharacterAnimation(CharacterAnimationState.Idle, CharacterAnimationDirection.Down);
        SetUpAnimation();
    }

    private void Start()
    {
        TimeTickSystem.OnTickMicro += TimeTickSystem_OnTickMicro;
    }

    private void TimeTickSystem_OnTickMicro(object sender, TimeTickSystem.OnTickEventArgs e)
    {
        if (currentTick >= currentCharacterAnimation.microTicksToNewFrame)
        {
            frameNumber++;
            if (frameNumber >= currentCharacterAnimation.animation.Length)
                frameNumber = 0;
            spriteRenderer.sprite = currentCharacterAnimation.animation[frameNumber];
            currentTick = 0;
        }
        else
            currentTick++;
    }

    public void ChangeAnimation(CharacterAnimationState state, CharacterAnimationDirection direction=CharacterAnimationDirection.Down)
    {
        if (currentCharacterAnimation.characterAnimationState != state || currentCharacterAnimation.characterAnimationDirection!=direction)
        {
            currentCharacterAnimation = GetCharacterAnimation(state, direction);
            SetUpAnimation();
        }
    }

    private void SetUpAnimation()
    {
        frameNumber = 0;
        spriteRenderer.flipX = currentCharacterAnimation.flipX;
        timer = 0;
        currentTick = 0;
        //timerMax = currentCharacterAnimation.frameRate;
    }

    public void GetAnimationFromVector2(float x, float y, CharacterAnimationState animationState)
    {
        CharacterAnimationDirection direction = CharacterAnimationDirection.Down;
        if (x == 0 && y != 0)
        {
            if ((y > 0))
                direction = CharacterAnimationDirection.Up;
            else
                direction = CharacterAnimationDirection.Down;
        }
        else
        {
            if (x < 0)
            {
                x *= -1;
                direction = CharacterAnimationDirection.Left;
            }
            else
                direction = CharacterAnimationDirection.Right;

            if (y > x || (y < 0 && y + x < 0))
            {
                if ((y > 0))
                    direction = CharacterAnimationDirection.Up;
                else
                    direction = CharacterAnimationDirection.Down;
            }
        }
        //else
        //ChangeAnimation(state);
        ChangeAnimation(animationState, direction);
    }

    private CharacterAnimation GetCharacterAnimation(CharacterAnimationState state, CharacterAnimationDirection direction=CharacterAnimationDirection.Down)
    {
        for (int i = 0; i < characterAnimations.Length; i++)
        {
            if (characterAnimations[i].characterAnimationState == state && characterAnimations[i].characterAnimationDirection==direction)
                return characterAnimations[i];
        }
        return characterAnimations[0];
    }

    public void Disable()
    {
        TimeTickSystem.OnTickMicro -= TimeTickSystem_OnTickMicro;
        this.enabled = false;
    }
}

[System.Serializable]
public struct CharacterAnimation
{
    public string characterAnimationName;
    public CharacterAnimationState characterAnimationState;
    public CharacterAnimationDirection characterAnimationDirection;
    public int microTicksToNewFrame;
    public Sprite[] animation;
    public bool flipX;
}
