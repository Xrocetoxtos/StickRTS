using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera camera;
    public Player player;
    public Grid grid;
    public Pathfinding pathfinding;

    [SerializeField] private LayerMask thingsMask;
    public LayerMask resourcesMask;

    public bool selectedPanelOpen = false;

    public static GameManager instance;

    private void Awake()
    {
        camera = Camera.main;
        instance = this;
    }

    private void Start()
    {
        TimeTickSystem.OnTick += TimeTickSystem_OnTick;
    }

    private void TimeTickSystem_OnTick(object sender, TimeTickSystem.OnTickEventArgs e)
    {
        //elke .2 seconde uitvoeren
        CheckMouseObject();
    }

    private void CheckMouseObject()
    {
        Vector2 mousePosition = BigBookBasic.MousePosition();
        Collider2D col = Physics2D.OverlapCircle(mousePosition, .01f, thingsMask);
        if (col != null)
            GUIManager.instance.TooltipObject(col.gameObject);
        else
            GUIManager.instance.TooltipObject(col);
    }
}
