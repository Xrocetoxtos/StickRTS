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

    public Transform[] slots = new Transform[8];
    public Character[] slotCharacters = new Character[8];
    [SerializeField] private float slotDistanceFromObject = .72f;

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
        CreateSlotTransforms();
    }

    private void CreateSlotTransforms()
    {
        slots[0] = CreateSlot(new Vector3(-slotDistanceFromObject, -slotDistanceFromObject));
        slots[1] = CreateSlot(new Vector3(-slotDistanceFromObject, 0));
        slots[2] = CreateSlot(new Vector3(-slotDistanceFromObject, slotDistanceFromObject));
        slots[3] = CreateSlot(new Vector3(0, -slotDistanceFromObject));
        slots[4] = CreateSlot(new Vector3(0, slotDistanceFromObject));
        slots[5] = CreateSlot(new Vector3(slotDistanceFromObject, -slotDistanceFromObject));
        slots[6] = CreateSlot(new Vector3(slotDistanceFromObject, 0));
        slots[7] = CreateSlot(new Vector3(slotDistanceFromObject, slotDistanceFromObject));
    }

    private Transform CreateSlot(Vector3 vector3)
    {
        GameObject slot = new GameObject("Slot");
        slot.transform.localScale = Vector3.one*2;
        slot.transform.position = transform.position + vector3;
        slot.transform.parent = transform;
        return slot.transform;
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
