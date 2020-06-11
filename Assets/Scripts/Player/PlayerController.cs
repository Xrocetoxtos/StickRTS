using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerSelection playerSelection;
    private Player player;

    private void Awake()
    {
        playerSelection = GetComponent<PlayerSelection>();
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && playerSelection.HasWorldoObjectsSelected())
        {
            if (playerSelection.selectedUnits.Count > 0)
            {
                playerSelection.RemoveNullObjectsFromSelection();
                WorldObject[] selectedObjects = playerSelection.GetSelectedObjects();
                MovementController[] selectedUnits = new MovementController[selectedObjects.Length];


                if (selectedObjects[0].worldObjectType == ObjectType.Character)
                {
                    for (int i = 0; i < selectedObjects.Length; i++)
                    {
                        selectedUnits[i] = BigBookBasic.GetComponentFromGameObject<MovementController>(selectedObjects[i].gameObject);
                    }
                }

                Vector2 mousePosition = BigBookBasic.MousePosition();
                Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePosition, .05f);
                Collider2D col = BigBookBasic.PickProminentCollider(colliders);
                if (col != null)
                {
                    if (col.gameObject.CompareTag("Ground"))
                    {
                        MoveToPosition(mousePosition, selectedUnits);
                    }
                    else if (col.gameObject.CompareTag("Water"))
                    {

                    }
                    else if (col.gameObject.CompareTag("Resource"))
                    {
                        MoveToObject(col.gameObject, selectedUnits);
                        //ook iets meegeven aan alle betrokken Units zodat ze ook iets gaan doen met die resource
                    }
                    else if (col.gameObject.CompareTag("Character"))
                    {
                        MoveToObject(col.gameObject, selectedUnits);

                    }
                    else if (col.gameObject.CompareTag("Building"))
                    {
                        MoveToObject(col.gameObject, selectedUnits);

                    }
                }
            }
        }
    }

    private void MoveToPosition(Vector2 position, MovementController[] units)
    {
        Vector2[] destinations = Positioning.GetUnitGroupDestinations(position, units.Length, .5f);
        for (int i = 0; i < units.Length; i++)
        {
            units[i].MoveToGround(destinations[i]);
        }
    }

    public void MoveToObject(GameObject target, MovementController[] units)
    {
        if (target.TryGetComponent(out WorldObject worldObject))
        {
            MovementController[] possibleUnits = SkipImpossible(worldObject, units);  // om characters die geen interactie kunnen hebben met het object uit te sluiten.
            Vector2[] positions = Positioning.GetCurrentPositions(possibleUnits); // voor als ze niet kunnen verplaatsen (geen ruimte)
            Vector2[] destinations = Positioning.GetUnitGroupDestinationsAroundWorldObject(worldObject, positions, possibleUnits); // bepalen van positie rond object
            SetTarget(worldObject, possibleUnits, destinations); // naar opject verplaatsen en AI instellen voor interactie
        }
        else
        {
            Vector2[] positions = Positioning.GetCurrentPositions(units);
            for (int i = 0; i < units.Length; i++)
            {
                units[i].MoveToPosition(positions[i]);
                units[i].ac.actualTarget = null;
            }
        }
    }

    private MovementController[] SkipImpossible(WorldObject worldObject, MovementController[] units)
    {
        List<MovementController> possibleUnits = new List<MovementController>();

        units = OnlyOwnObjects<MovementController>(units).ToArray(); 

        switch (worldObject.worldObjectType)
        {
            case ObjectType.Building:
                return units;
            case ObjectType.Character:
                return units;
            case ObjectType.Resource:
                {
                    //alleen units die uberhaupt resources kunnen dragen kunnen interactie met resources hebben.
                    for (int u = 0; u < units.Length; u++)
                    {
                        WorldObject unitWo = units[u].GetComponent<WorldObject>();
                        if (unitWo != null)
                        {
                            if (unitWo.player == player)
                            {
                                GatherController gather = units[u].GetComponent<GatherController>();
                                if (gather != null)
                                {
                                    if (gather.maxResourceAmount > 0)
                                        possibleUnits.Add(units[u]);
                                }
                            }
                        }
                    }
                    break;
                }
        }
        return possibleUnits.ToArray();
    }

    private List<T> OnlyOwnObjects<T>(T[] inputList) where T:MonoBehaviour
    {
        List<T> outputList = new List<T>();
        for (int u = 0; u < inputList.Length; u++)
        {
            WorldObject unitWo = inputList[u].GetComponent<WorldObject>();
            if (unitWo != null)
            {
                if (unitWo.player == player)
                {
                    outputList.Add(inputList[u]);
                }
            }
        }
        return outputList;
    }

    private void SetTarget(WorldObject worldObject, MovementController[] units, Vector2[] positions)
    {
        switch (worldObject.worldObjectType)
        {
            case ObjectType.Building:
                for (int i = 0; i < units.Length; i++)
                {
                    units[i].MoveToWorldObject(worldObject, positions[i], true);
                }
                break;
            case ObjectType.Character:
                for (int i = 0; i < units.Length; i++)
                {
                    if (units[i].ac.animator!=null)
                        units[i].ac.animator.SetBool("Gathering", false);
                    units[i].MoveToWorldObject(worldObject, positions[i], true);
                }
                break;
            case ObjectType.Resource:
                {
                    Resource resource = BigBookBasic.GetComponentFromGameObject<Resource>(worldObject.gameObject);
                    if (resource != null)
                    {
                        for (int i = 0; i < units.Length; i++)
                        {
                            if (units[i].ac.animator != null)
                            {
                                units[i].ac.animator.SetBool("IsGathering", true);
                                if (resource.available)
                                    units[i].ac.animator.SetBool("IsAttacking", false);
                                else
                                    units[i].ac.animator.SetBool("IsAttacking", true);
                            }
                            units[i].MoveToWorldObject(worldObject, positions[i], true, true);
                        }
                    }
                    else
                    {
                        Debug.Log("geen resource: " + worldObject.worldObjectName);
                    }
                    break;
                }
        }
    }
}
