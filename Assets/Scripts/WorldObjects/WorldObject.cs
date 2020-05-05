using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    public ObjectType worldObjectType;
    public Player player;
    public string worldObjectName;
    public string worldObjectDescription;
    public Sprite worldObjectSprite;

    public GameObject worldObjectSelectionVisual;

    private int currentHealth;
    [SerializeField] private int maxHealth=10;

    [SerializeField] private List<WorldObjectAction> allWorldObjectActions = new List<WorldObjectAction>();
    public List<WorldObjectAction> allowedWorldObjectActions = new List<WorldObjectAction>();

    public event EventHandler OnHealthChanged;

    protected virtual void Awake()
    {
        if (player == null)
            player = GameManager.instance.player;

        currentHealth = maxHealth;
        foreach(WorldObjectAction action in allWorldObjectActions)
        {
            if (player.ActionAvailable(action))
                allowedWorldObjectActions.Add(action);
        }
    }

    public float HealthPerunage()
    {
        return (float)currentHealth / maxHealth;
    }

    public void ChangeHealth(int change)
    {
        currentHealth = Mathf.Clamp(currentHealth + change, 0, maxHealth);
        if (OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
        if (currentHealth == 0)
            Die();
    }

    private void Die()
    {
        Debug.Log(worldObjectName + " died.");
    }

    public void ToggleSelectionVisual(bool visual)
    {
        if (worldObjectSelectionVisual != null)
            worldObjectSelectionVisual.SetActive(visual);
    }
}
