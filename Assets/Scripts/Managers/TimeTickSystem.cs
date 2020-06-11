﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTickSystem : MonoBehaviour
{
    public class OnTickEventArgs:EventArgs
    {
        public int tick;
    }

    public static event EventHandler<OnTickEventArgs> OnTickMicro;
    public static event EventHandler<OnTickEventArgs> OnTick;
    public static event EventHandler<OnTickEventArgs> OnTick_5;

    private const float TICK_TIMER_MICRO = .01f;
    private const float TICK_TIMER_MAX = .2f;

    private int tick;
    private float tickTimer;
    private float tickTimerMicro;


    private void Awake()
    {
        tick = 0;
    }

    private void Update()
    {
        tickTimerMicro += Time.deltaTime;
        tickTimer += tickTimerMicro;
        if(tickTimerMicro>=TICK_TIMER_MICRO)
        {
            tickTimerMicro -= TICK_TIMER_MICRO;
            if (OnTickMicro != null) OnTickMicro(this, new OnTickEventArgs { tick = tick });
        }     
        if(tickTimer >=TICK_TIMER_MAX)
        {
            tickTimer -= TICK_TIMER_MAX;
            tick++;
            if (OnTick != null) OnTick(this, new OnTickEventArgs { tick = tick });

            if (tick%5 ==0)
                if (OnTick_5 != null) OnTick_5(this, new OnTickEventArgs { tick = tick });
        }
    }
}
