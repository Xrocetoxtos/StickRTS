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
            Collider2D col = Physics2D.OverlapCircle(mousePosition, .05f);
            if (col != null)
            {

                if (col.gameObject.CompareTag("Ground"))
                {
                    MoveToPosition(mousePosition, selectedCharacters);
                }
                else if (col.gameObject.CompareTag("Resource"))
                {

                }
                else if (col.gameObject.CompareTag("Character"))
                {

                }
                else if (col.gameObject.CompareTag("Building"))
                {

                }
            }
        }
    }

    private void MoveToPosition(Vector2 position, Character[] characters)
    {
        Vector2[] destinations = Positioning.GetUnitGroupDestinations(position, characters.Length, .5f);
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].MoveToPosition(destinations[i]);
        }
    }
}
