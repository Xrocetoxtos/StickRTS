using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WorldObjectDisplay : MonoBehaviour
{
    private WorldObject worldObject;
    private SelectedPanel panel;

    [SerializeField] private TextMeshProUGUI worldObjectNameText;
    [SerializeField] private TextMeshProUGUI worldObjectDescriptionText;
    [SerializeField] private Image worldObjectImage;
    [SerializeField] private TextMeshProUGUI worldObjectPlayerNameText;

    [SerializeField] private GameObject healthBarObject;
    [SerializeField] private Image worldObjectHealthBar;

    [SerializeField] private GameObject actionsDisplayObject;
    [SerializeField] private Transform actionDisplayContainer;
    [SerializeField] private GameObject actionsDisplayPrefab;

    public void Setup(WorldObject _worldObject, SelectedPanel _panel=null)
    {
        panel = _panel;
        if (_worldObject)
        {
            worldObject = _worldObject;
            worldObject.OnHealthChanged += WorldObject_OnHealthChanged;


            if (worldObjectNameText != null)
                worldObjectNameText.SetText(worldObject.worldObjectName);
            if (worldObjectDescriptionText != null)
                worldObjectDescriptionText.SetText(worldObject.worldObjectDescription);
            if (worldObjectImage != null)
                worldObjectImage.sprite = worldObject.worldObjectSprite;
            if (worldObjectPlayerNameText != null)
                worldObjectPlayerNameText.SetText("("+ worldObject.player.playerName + ")");
            if (worldObjectHealthBar != null)
                SetUpHealthBar();
            else
                RemoveHealthBar();

            SetupActions();
        }
    }


    public void Empty()
    {
        if (worldObjectNameText != null)
            worldObjectNameText.SetText("");
        if (worldObjectDescriptionText != null)
            worldObjectDescriptionText.SetText("");
        if (worldObjectImage != null)
            worldObjectImage.sprite = null;
            RemoveHealthBar();
    }

    private void RemoveHealthBar()
    {
        if(healthBarObject)
            healthBarObject.SetActive(false);
    }

    private void SetUpHealthBar()
    {
        healthBarObject.SetActive(true);
        worldObjectHealthBar.fillAmount = worldObject.HealthPerunage();
    }

    private void WorldObject_OnHealthChanged(object sender, System.EventArgs e)
    {
        //bijwerken healthbar
        SetUpHealthBar();
    }

    private void SetupActions()
    {
        if (actionDisplayContainer == null || actionsDisplayPrefab == null)
            return;

        foreach (Transform child in actionDisplayContainer)
            Destroy(child.gameObject);

        if (worldObject.allowedWorldObjectActions.Count > 0)
        {
            actionsDisplayObject.SetActive(true);
            foreach (WorldObjectAction action in worldObject.allowedWorldObjectActions)
            {
                GameObject display = Instantiate(actionsDisplayPrefab, actionDisplayContainer);
                display.GetComponent<WorldObjectActionDisplay>().Setup(action, worldObject);
            }
        }
        else
        {
            actionsDisplayObject.SetActive(false);
        }
    }

    public void PickThisWorldObject()
    {
        Debug.Log("klik");
        if(panel!=null)
        {
            panel.ClearMultiples();
            panel.playerSelection.DeselectAll();
            panel.playerSelection.ChangeSpecificWorldObject(worldObject);
        }
    }
}
