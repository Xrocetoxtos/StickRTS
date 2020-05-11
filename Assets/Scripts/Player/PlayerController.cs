using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerSelection playerSelection;

    private void Awake()
    {
        playerSelection = GetComponent<PlayerSelection>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && playerSelection.HasWorldoObjectsSelected())
        {
            playerSelection.RemoveNullObjectsFromSelection();
            WorldObject[] selectedObjects = playerSelection.GetSelectedObjects();
            Character[] selectedCharacters = new Character[selectedObjects.Length];

            if (selectedObjects[0].worldObjectType == ObjectType.Character)
            {

                for (int i = 0; i < selectedObjects.Length; i++)
                {
                    selectedCharacters[i] = BigBookBasic.GetComponentFromGameObject<Character>(selectedObjects[i].gameObject);
                }
            }

            Vector2 mousePosition = BigBookBasic.MousePosition();
            Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePosition, .05f);
            Collider2D col = BigBookBasic.PickProminentCollider(colliders);
            if (col != null)
            {
                if (col.gameObject.CompareTag("Ground"))
                {
                    MoveToPosition(mousePosition, selectedCharacters);
                }
                else if (col.gameObject.CompareTag("Water"))
                {

                }
                else if (col.gameObject.CompareTag("Resource"))
                {
                    MoveToObject(col.gameObject, selectedCharacters);
                    //ook iets meegeven aan alle betrokken characters zodat ze ook iets gaan doen met die resource
                }
                else if (col.gameObject.CompareTag("Character"))
                {
                    MoveToObject(col.gameObject, selectedCharacters);

                }
                else if (col.gameObject.CompareTag("Building"))
                {
                    MoveToObject(col.gameObject, selectedCharacters);

                }
            }
        }
    }


    private void MoveToPosition(Vector2 position, Character[] characters)
    {
        Vector2[] destinations = Positioning.GetUnitGroupDestinations(position, characters.Length, .5f);
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].MoveToGround(destinations[i]);
        }
    }

    public void MoveToObject(GameObject target, Character[] characters)
    {
        if (target.TryGetComponent(out WorldObject worldObject))
        {
            Character[] possibleCharacters = SkipImpossible(worldObject, characters);  // om characters die geen interactie kunnen hebben met het object uit te sluiten.
            Vector2[] positions = Positioning.GetCurrentPositions(possibleCharacters); // voor als ze niet kunnen verplaatsen (geen ruimte)
            Vector2[] destinations = Positioning.GetUnitGroupDestinationsAroundWorldObject(worldObject, positions, possibleCharacters); // bepalen van positie rond object
            SetTarget(worldObject, possibleCharacters, destinations); // naar opject verplaatsen en AI instellen voor interactie
        }
        else
        {
            Vector2[] positions = Positioning.GetCurrentPositions(characters);
            for (int i = 0; i < characters.Length; i++)
            {
                characters[i].MoveToPosition(positions[i]);
                characters[i].actualTarget = null;
            }
        }
    }

    private Character[] SkipImpossible(WorldObject worldObject, Character[] characters)
    {
        List<Character> possibleCharacters = new List<Character>();

        switch (worldObject.worldObjectType)
        {
            case ObjectType.Building:
                return characters;
            case ObjectType.Character:
                return characters;
            case ObjectType.Resource:
                {
                    //alleen units die uberhaupt resources kunnen dragen kunnen interactie met resources hebben.
                    for (int c = 0; c < characters.Length; c++)
                    {
                        if (characters[c].maxResourceAmount > 0)
                            possibleCharacters.Add(characters[c]);
                    }
                    break;
                }
        }
        return possibleCharacters.ToArray();

    }

    private void SetTarget(WorldObject worldObject, Character[] characters, Vector2[] positions)
    {
        switch (worldObject.worldObjectType)
        {
            case ObjectType.Building:
                for (int i = 0; i < characters.Length; i++)
                {
                    characters[i].DeliverResource(worldObject, positions[i]);
                }
                break;
            case ObjectType.Character:
                for (int i = 0; i < characters.Length; i++)
                {
                    characters[i].actualTarget = worldObject;
                }
                break;
            case ObjectType.Resource:
                {
                    for (int i = 0; i < characters.Length; i++)
                    {
                        characters[i].GatherResource(worldObject, positions[i]);
                    }
                    break;
                }
        }
    }
}
