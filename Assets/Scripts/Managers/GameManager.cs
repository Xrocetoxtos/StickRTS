using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera camera;
    public Player player;
    public Grid grid;

    [SerializeField] private LayerMask thingsMask;
    private float timer = 0f;
    private float timerMax = .3f;

    public bool selectedPanelOpen = false;

    public static GameManager instance;

    private void Awake()
    {
        camera = Camera.main;
        instance = this;
    }

    private void LateUpdate()
    {
        if (timer >= timerMax)
        {
            timer -= timerMax;
            Vector2 mousePosition = BigBookBasic.MousePosition();
            Collider2D col = Physics2D.OverlapCircle(mousePosition, .01f, thingsMask);
            if (col != null)
                GUIManager.instance.TooltipObject(col.gameObject);
            else
                GUIManager.instance.TooltipObject(col);
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

}
