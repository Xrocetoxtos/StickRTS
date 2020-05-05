using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldObjectActionDisplay : MonoBehaviour
{
    private WorldObjectAction action;

    [SerializeField] private Image actionImage;

    private void Awake()
    {
        actionImage = GetComponent<Image>();
    }

    public void Setup(WorldObjectAction _action)
    {
        if (_action != null)
        {
            action = _action;

            if (actionImage != null)
                actionImage.sprite = action.actionSprite;
        }
    }
}
