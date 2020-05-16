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

    [SerializeField] private Image resourceImage;
    [SerializeField] private TextMeshProUGUI resourceAmountText;
    [SerializeField] private GameObject resourceBarObject;
    [SerializeField] private Image resourceBar;

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
                worldObjectPlayerNameText.SetText("(" + worldObject.player.playerName + ")");
            if (worldObjectHealthBar != null)
                SetUpHealthBar();
            else
                RemoveHealthBar();

            switch (worldObject.worldObjectType)
            {
                case ObjectType.Character:
                    break;
                case ObjectType.Building:
                    break;
                case ObjectType.Resource:
                    SetupResource();
                    break;
            }
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
        if (resourceImage != null)
            resourceImage.sprite = null;
        if (resourceAmountText != null)
            resourceAmountText.SetText("");
        RemoveHealthBar();
    }

    private void RemoveHealthBar()
    {
        if(healthBarObject)
            healthBarObject.SetActive(false);
    }

    private void SetUpHealthBar()
    {
        if (healthBarObject != null)
        {
            healthBarObject.SetActive(true);
            worldObjectHealthBar.fillAmount = worldObject.HealthPerunage();
        }
    }

    private void WorldObject_OnHealthChanged(object sender, System.EventArgs e)
    {
        //bijwerken healthbar
        SetUpHealthBar();
    }

    private void SetupResource()
    {
        Resource resource = BigBookBasic.GetComponentFromGameObject<Resource>(worldObject.gameObject);
        if (resourceImage != null)
            resourceImage.sprite = GUIManager.instance.GetResouceSprite(resource.resourceType);
        if (resourceAmountText != null)
        {
            int am = (int)resource.resourceAmount;
            int ma = (int)resource.maxResourceAmount;
            resourceAmountText.SetText(am.ToString() + "/" + ma.ToString());
        }
        if (resourceBarObject != null)
        {
            resourceBarObject.SetActive(true);
            resourceBar.fillAmount = resource.GetResourcePerunage();
        }
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
        if(panel!=null)
        {
            panel.ClearMultiples();
            panel.playerSelection.DeselectAll();
            panel.playerSelection.ChangeSpecificWorldObject(worldObject);
        }
    }
}
