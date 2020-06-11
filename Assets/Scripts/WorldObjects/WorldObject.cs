using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    [Header("Info")]
    public ObjectType worldObjectType;
    public Player player;
    public string worldObjectName;
    public string worldObjectDescription;
    public Sprite worldObjectSprite;

    [Header("Components")]
    public AIController aIController;
    public MovementController movement;


    public GameObject worldObjectSelectionVisual;

    private int currentHealth;
    [SerializeField] private int maxHealth=10;
    public Sprite deadSprite;

    [SerializeField] private List<WorldObjectAction> allWorldObjectActions = new List<WorldObjectAction>();
    public List<WorldObjectAction> allowedWorldObjectActions = new List<WorldObjectAction>();

    public Transform[] slots = new Transform[8];
    public Unit[] slotCharacters = new Unit[8];
    [SerializeField] private float slotDistanceFromObject = .72f;

    public CharacterAnimationState killAnimation=CharacterAnimationState.FightStab;

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

        movement = GetComponent<MovementController>();
        aIController = GetComponent<AIController>();
    }

    protected virtual void Start()
    {

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

    protected virtual void Die()
    {
        Debug.Log(worldObjectName + " died.");
        if(deadSprite!=null)
        {
            TryGetComponent<SpriteRenderer>(out SpriteRenderer sr);
            if(sr!=null)
            {
                sr.sprite = deadSprite;
                
            }
        }
        TryGetComponent<Animator>(out Animator anim);
        if (anim != null) anim.SetTrigger("Death");
        //if(aIController)
        //    aIController.enabled = false;
        //if(movement)
        //    movement.enabled = false;
        //TryGetComponent<CharacterAnimator>(out CharacterAnimator ca);
        //if (ca != null)
        //    ca.Disable();
        //TryGetComponent<GatherController>(out GatherController gc);
        //if (gc != null)
        //    gc.enabled = false;
    }

    public void ToggleSelectionVisual(bool visual)
    {
        if (worldObjectSelectionVisual != null)
            worldObjectSelectionVisual.SetActive(visual);
    }
}
