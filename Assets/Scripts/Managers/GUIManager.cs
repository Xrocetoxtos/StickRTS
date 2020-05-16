using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIManager : MonoBehaviour
{
    private GameObject tooltipPanel;
    private WorldObjectDisplayShell tooltipDisplayShell;

    [SerializeField] private TextMeshProUGUI woodText, foodText, goldText, stoneText;
    [SerializeField] private Sprite woodSprite, foodSprite, goldSprite, stoneSprite;
    private GameObject selectedPanelObject;
    private SelectedPanel selectedPanel;

    public static GUIManager instance;

    private void Awake()
    {
        instance = this;

        tooltipPanel = GameObject.Find("Tooltip");
        tooltipDisplayShell = tooltipPanel.GetComponent<WorldObjectDisplayShell>();
        selectedPanelObject = GameObject.Find("SelectedObjectsPanel");
        selectedPanel = selectedPanelObject.GetComponent<SelectedPanel>();
        selectedPanelObject.SetActive(false);

        GameManager.instance.player.OnResourcesChanged += Player_OnResourcesChanged;
    }

    private void Player_OnResourcesChanged(object sender, EventArgs e)
    {
        foodText.SetText(GameManager.instance.player.GetResource(ResourceType.Food).ToString());
        woodText.SetText(GameManager.instance.player.GetResource(ResourceType.Wood).ToString());
        goldText.SetText(GameManager.instance.player.GetResource(ResourceType.Gold).ToString());
        stoneText.SetText(GameManager.instance.player.GetResource(ResourceType.Stone).ToString());
    }

    public void TooltipObject(GameObject go)
    {
        tooltipPanel.SetActive(true);
        tooltipDisplayShell.Setup(BigBookBasic.GetComponentFromGameObject<WorldObject>(go));
        tooltipPanel.transform.position = TooltipPosition();
    }

    private Vector3 TooltipPosition()
    {
        Vector3 position = Input.mousePosition+Vector3.up*30;
        if (position.x - 80 < 0) position = new Vector3(80, position.y, 0);
        else if (position.x + 80 > Screen.width) position = new Vector3(Screen.width - 80, position.y, 0);

        if (position.y - 15 < 0) position = new Vector3(position.x, 15, 0);
        else if (position.y+15 > Screen.height) position = new Vector3(position.x, Screen.height - 15, 0);
        return position;
    }

    public void TooltipObject(Collider2D col)
    {
        if (!col)
            tooltipPanel.SetActive(false);
    }

    public void UpdateSelectedPanel(PlayerSelection selection)
    {
        if (selection.HasWorldoObjectsSelected())
        {
            selectedPanelObject.SetActive(true);
            selectedPanel.Setup(selection);
            GameManager.instance.selectedPanelOpen = true;
        }
        else
        {
            selectedPanelObject.SetActive(false);
            GameManager.instance.selectedPanelOpen = false;
        }
    }

    public Sprite GetResouceSprite(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Food:
                return foodSprite;
            case ResourceType.Wood:
                return woodSprite;
            case ResourceType.Stone:
                return stoneSprite;
            case ResourceType.Gold:
                return goldSprite;
            case ResourceType.None:
                return null;
        }
        return null;
    }
}
