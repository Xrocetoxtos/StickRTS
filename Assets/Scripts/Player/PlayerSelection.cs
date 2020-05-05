﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelection : MonoBehaviour
{
    private const float SELECTION_HEIGHT = 120f;

    private Player player;

    private RectTransform selectionBox;
    [SerializeField] private LayerMask selectablesMask;

    private Vector2 startPos;
    public bool selectionChanged = false;

    public List<WorldObject> selectedCharacters = new List<WorldObject>();
    public List<WorldObject> selectedNonCharacters = new List<WorldObject>();

    private void Awake()
    {
        player = GetComponent<Player>();
        selectionBox = GameObject.Find("SelectionBox").GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (selectionChanged)
        {
            ToggleVisuals();
            GUIManager.instance.UpdateSelectedPanel(this);
            selectionChanged = false;
        }

        if ((!GameManager.instance.selectedPanelOpen || Input.mousePosition.y > SELECTION_HEIGHT))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("klik!!  " + Input.mousePosition.y);
                StartDrawingSelectionBox();
                if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))
                    DeselectAll();

                AddClickToSelection();
                selectionChanged = true;
            }

            if (Input.GetMouseButton(0))
            {
                UpdateSelectionBox();
            }

            if (Input.GetMouseButtonUp(0))
            {
                FinishSelectionBox();
            }
        }
    }

    private void StartDrawingSelectionBox()
    {
        startPos = Input.mousePosition;
    }
    private void UpdateSelectionBox()
    {
        Vector2 curMousePosition = Input.mousePosition;
        if (!selectionBox.gameObject.activeInHierarchy)
        {
            selectionBox.gameObject.SetActive(true);
        }
        float width = curMousePosition.x - startPos.x;
        float height = curMousePosition.y - startPos.y;

        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
    }
    private void FinishSelectionBox()
    {
        selectionBox.gameObject.SetActive(false);

        Vector2 min = GameManager.instance.camera.ScreenToWorldPoint(selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2));
        Vector2 max = GameManager.instance.camera.ScreenToWorldPoint(selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2));

        //eerst checken of er characters in de range zitten. Die krijgen voorrang
        foreach (WorldObject character in player.characters)
        {
            Vector2 pos = character.transform.position;
            if (pos.x > min.x && pos.x < max.x && pos.y > min.y && pos.y < max.y)
            {
                AddSpecificWorldObject(character);
            }
        }
        PrioritiseCharacters();
    }

    // =======================================================================

    private void DeselectAll()
    {
        foreach (WorldObject worldObject in selectedCharacters)
        {
            worldObject.ToggleSelectionVisual(false);
        }
        foreach (WorldObject worldObject in selectedNonCharacters)
        {
            worldObject.ToggleSelectionVisual(false);
        }
        selectedCharacters.Clear();
        selectedNonCharacters.Clear();
    }

    private void AddClickToSelection()
    {
        Vector2 screenPos = BigBookBasic.MousePosition();
        Collider2D col = Physics2D.OverlapCircle(screenPos, .01f, selectablesMask);

        if (col != null)
        {
            AddSpecificWorldObject(BigBookBasic.GetComponentFromGameObject<WorldObject>(col.gameObject));
        }
    }

    public void AddSpecificWorldObject(WorldObject worldObject)
    {
        if (worldObject.worldObjectType == ObjectType.Character)
        {
            if (!selectedCharacters.Contains(worldObject))
            {
                selectedCharacters.Add(worldObject);
                worldObject.ToggleSelectionVisual(true);
            }
            else
            {
                selectedCharacters.Remove(worldObject);
                worldObject.ToggleSelectionVisual(false);
            }
        }
        else
        {
            if (!selectedNonCharacters.Contains(worldObject))
            {
                foreach (WorldObject wo in selectedNonCharacters)
                {
                    if (wo !=worldObject)
                        wo.ToggleSelectionVisual(false);
                }
                selectedNonCharacters.Clear();
                selectedNonCharacters.Add(worldObject);
                worldObject.ToggleSelectionVisual(true);
            }
            else
            {
                selectedNonCharacters.Clear();
                worldObject.ToggleSelectionVisual(false);
            }

        }
        selectionChanged = true;
    }
    private void PrioritiseCharacters()
    {
        if (selectedCharacters.Count > 0)
            selectedNonCharacters.Clear();
    }

    public bool HasWorldoObjectsSelected()
    {
        return selectedCharacters.Count > 0 || selectedNonCharacters.Count > 0;
    }
    public WorldObject[] GetSelectedObjects()
    {
        if (selectedCharacters.Count > 0)
            return selectedCharacters.ToArray();
        else
            return selectedNonCharacters.ToArray();
    }

    public void RemoveNullObjectsFromSelection()
    {
        for (int x = 0; x < selectedCharacters.Count; x++)
        {
            if (selectedCharacters[x] == null)
            {
                selectedCharacters.RemoveAt(x);
                selectionChanged = true;
            }
        }
        for (int x = 0; x < selectedNonCharacters.Count; x++)
        {
            if (selectedNonCharacters[x] == null)
            {
                selectedNonCharacters.RemoveAt(x);
                selectionChanged = true;
            }
        }
    }

    private void ToggleVisuals()
    {
        foreach (WorldObject worldObject in selectedCharacters)
        {
            worldObject.ToggleSelectionVisual(true);
        }
        foreach (WorldObject worldObject in selectedNonCharacters)
        {
            worldObject.ToggleSelectionVisual(true);
        }
    }
}



//    if (Input.GetMouseButtonDown(0) && (!GameManager.instance.selectedPanelOpen || Input.mousePosition.y>120))
//    {
//        ToggleSelectionVisual(false);
//        selectedCharacterList = new List<WorldObject>();
//        selectedObject = null;
//        startPos = Input.mousePosition;
//        TrySelect(BigBookBasic.MousePosition());
//    }

//    if (Input.GetMouseButtonUp(0))
//    {
//        ReleaseSelectionBox();
//    }

//    if (Input.GetMouseButton(0) && (!GameManager.instance.selectedPanelOpen || Input.mousePosition.y > 120))
//    {
//        UpdateSelectionBox(Input.mousePosition);
//    }
//}



//void UpdateSelectionBox(Vector2 curMousePosition)
//{
//    if (!selectionBox.gameObject.activeInHierarchy)
//    {
//        selectionBox.gameObject.SetActive(true);
//    }
//    float width = curMousePosition.x - startPos.x;
//    float height = curMousePosition.y - startPos.y;

//    selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);
//    selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
//}

//private void ReleaseSelectionBox()
//{
//    selectionBox.gameObject.SetActive(false);

//    Vector2 min = GameManager.instance.camera.ScreenToWorldPoint(selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2));
//    Vector2 max = GameManager.instance.camera.ScreenToWorldPoint(selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2));

//    //eerst checken of er characters in de range zitten. Die krijgen voorrang
//    foreach (WorldObject character in player.characters)
//    {
//        Vector2 pos = character.transform.position;
//        if (pos.x > min.x && pos.x < max.x && pos.y > min.y && pos.y < max.y)
//        {
//            SelectSpecificWorldObject(character);
//        }
//    }
//    if (selectedCharacterList.Count > 0)
//        return;
//    else if (selectedObject != null)
//        SelectSpecificWorldObject(selectedObject);
//}

//public void TrySelect(Vector2 screenPos)
//{
//    selectedObject = null;
//    Collider2D col = Physics2D.OverlapCircle(screenPos, .01f, selectablesMask);

//    if (col != null)
//    {
//        selectedObject = BigBookBasic.GetComponentFromGameObject<WorldObject>(col.gameObject);
//    }
//}

//public void SelectSpecificWorldObject(WorldObject worldObject)
//{
//    selectedCharacterList.Add(worldObject);
//    worldObject.ToggleSelectionVisual(true);
//    selectionChanged = true;
//    if (OnSelectionChanged != null) OnSelectionChanged(this, EventArgs.Empty);

//}

//private void ToggleSelectionVisual(bool selected)
//{
//    foreach (WorldObject worldObject in selectedCharacterList)
//    {
//        worldObject.ToggleSelectionVisual(selected);
//    }
//}

//public bool HasWorldoObjectsSelected()
//{
//    return selectedCharacterList.Count > 0 || selectedObject!=null;
//}

//public WorldObject[] GetSelectedCharacters()
//{
//    if (selectedCharacterList.Count > 0)
//        return selectedCharacterList.ToArray();
//    else
//    {
//        WorldObject[] worldObjects = new WorldObject[1];
//        worldObjects[0] = selectedObject;
//        return worldObjects;
//    }
//}

