using System;
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

    public List<WorldObject> selectedUnits = new List<WorldObject>();
    public List<WorldObject> selectedNonUnits = new List<WorldObject>();

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
                ChangeSpecificWorldObject(character);
            }
        }
        PrioritiseCharacters();
    }

    // =======================================================================

    public void DeselectAll()
    {
        foreach (WorldObject worldObject in selectedUnits)
        {
            worldObject.ToggleSelectionVisual(false);
        }
        foreach (WorldObject worldObject in selectedNonUnits)
        {
            worldObject.ToggleSelectionVisual(false);
        }
        selectedUnits.Clear();
        selectedNonUnits.Clear();
    }

    private void AddClickToSelection()
    {
        Vector2 screenPos = BigBookBasic.MousePosition();
        Collider2D col = Physics2D.OverlapCircle(screenPos, .01f, selectablesMask);

        if (col != null)
        {
            Debug.Log(col.gameObject);

            ChangeSpecificWorldObject(BigBookBasic.GetComponentFromGameObject<WorldObject>(col.gameObject));
        }
    }

    public void ChangeSpecificWorldObject(WorldObject worldObject)
    {
        if (worldObject.worldObjectType == ObjectType.Character)
        {
            if (!selectedUnits.Contains(worldObject))
            {
                selectedUnits.Add(worldObject);
                worldObject.ToggleSelectionVisual(true);
            }
            else
            {
                selectedUnits.Remove(worldObject);
                worldObject.ToggleSelectionVisual(false);
            }
        }
        else
        {
            if (!selectedNonUnits.Contains(worldObject))
            {
                foreach (WorldObject wo in selectedNonUnits)
                {
                    if (wo !=worldObject)
                        wo.ToggleSelectionVisual(false);
                }
                selectedNonUnits.Clear();
                selectedNonUnits.Add(worldObject);
                worldObject.ToggleSelectionVisual(true);
            }
            else
            {
                selectedNonUnits.Clear();
                worldObject.ToggleSelectionVisual(false);
            }

        }
        selectionChanged = true;
    }
    private void PrioritiseCharacters()
    {
        if (selectedUnits.Count > 0)
            selectedNonUnits.Clear();
    }

    public bool HasWorldoObjectsSelected()
    {
        return selectedUnits.Count > 0 || selectedNonUnits.Count > 0;
    }
    public WorldObject[] GetSelectedObjects()
    {
        if (selectedUnits.Count > 0)
            return selectedUnits.ToArray();
        else
            return selectedNonUnits.ToArray();
    }

    public void RemoveNullObjectsFromSelection()
    {
        for (int x = 0; x < selectedUnits.Count; x++)
        {
            if (selectedUnits[x] == null)
            {
                selectedUnits.RemoveAt(x);
                selectionChanged = true;
            }
        }
        for (int x = 0; x < selectedNonUnits.Count; x++)
        {
            if (selectedNonUnits[x] == null)
            {
                selectedNonUnits.RemoveAt(x);
                selectionChanged = true;
            }
        }
    }

    private void ToggleVisuals()
    {
        foreach (WorldObject worldObject in selectedUnits)
        {
            worldObject.ToggleSelectionVisual(true);
        }
        foreach (WorldObject worldObject in selectedNonUnits)
        {
            worldObject.ToggleSelectionVisual(true);
        }
    }
}