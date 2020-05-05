using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GUIManager : MonoBehaviour
{
    private GameObject tooltipPanel;
    private WorldObjectDisplayShell tooltipDisplayShell;

    [SerializeField] private TextMeshProUGUI woodText, foodText, goldText, stoneText;

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
}
