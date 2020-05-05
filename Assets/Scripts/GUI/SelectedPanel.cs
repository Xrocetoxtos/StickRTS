using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedPanel : MonoBehaviour
{
    public PlayerSelection playerSelection;

    private WorldObject[] worldObjects;

    [SerializeField] private GameObject oneObject;
    [SerializeField] private GameObject toTenObjects;
    [SerializeField] private GameObject moreThanTenObjects;
    [SerializeField] private GameObject worldObjectDisplayPrefab;
    public Transform moreObjectsTransform;

    [SerializeField] private WorldObjectDisplay worldObjectDisplay;

    private void Awake()
    {
        playerSelection = GameManager.instance.player.playerSelection;
    }

    public void Setup(PlayerSelection selection)
    {
        playerSelection = selection;
        worldObjects = selection.GetSelectedObjects();

        if (worldObjects.Length == 1)
            SetupOneCharacter();
        else if (worldObjects.Length <= 10)
            SetupToTenCharacters();
        else if (worldObjects.Length>10)
            SetupMorecharacters();
    }

    private void SetupOneCharacter()
    {
        SelectPanel(oneObject);
        WorldObject worldObject = worldObjects[0];
        if(worldObject!=null)
            worldObjectDisplay.Setup(worldObject,this);
    }

    private void SetupToTenCharacters()
    {
        SelectPanel(toTenObjects);
        ClearMultiples();
        foreach(WorldObject worldObject in worldObjects)
        {
            GameObject display = Instantiate(worldObjectDisplayPrefab, moreObjectsTransform);
            display.GetComponent<WorldObjectDisplay>().Setup(worldObject, this);
        }
    }

    private void SetupMorecharacters()
    {
        SelectPanel(moreThanTenObjects);
    }

    // ==========================================

    private void SelectPanel(GameObject panel)
    {
        oneObject.SetActive(false);
        toTenObjects.SetActive(false);
        moreThanTenObjects.SetActive(false);

        panel.SetActive(true);
    }

    public void ClearMultiples()
    {
        foreach(Transform child in moreObjectsTransform)
        {
            Destroy(child.gameObject);
        }
    }
}
