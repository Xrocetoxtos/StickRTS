using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WorldObjectActionDisplay : MonoBehaviour
{
    private WorldObjectAction action;
    private WorldObject worldObject;

    [SerializeField] private Image actionImage;
    [SerializeField] private GameObject unavailableMask;

    private void Awake()
    {
        actionImage = GetComponent<Image>();
    }

    public void Setup(WorldObjectAction _action, WorldObject _worldObject)
    {

        if (_action != null)
        {
            action = _action;
            worldObject = _worldObject;

            worldObject.player.OnResourcesChanged += Player_OnResourcesChanged;

            if (actionImage != null)
                actionImage.sprite = action.actionSprite;
            if (unavailableMask != null)
                unavailableMask.SetActive(!action.IsAvailable(worldObject.player));

        }
    }

    private void Player_OnResourcesChanged(object sender, EventArgs e)
    {
        if (unavailableMask != null)
            unavailableMask.SetActive(!action.IsAvailable(worldObject.player));
    }

    public void Perform()
    {
        action.Perform(worldObject.player);
    }
}
