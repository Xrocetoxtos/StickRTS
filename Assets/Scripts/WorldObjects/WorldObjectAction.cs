using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[CreateAssetMenu(fileName ="new WorldObjectAction", menuName ="WorldObjectAction")]
public class WorldObjectAction : ScriptableObject
{
    public string actionName;
    public ActionType actionType;
    public Sprite actionSprite;

    public GameObject prefabToInstantiate;

    public int wood;
    public int food;
    public int gold;
    public int stone;

    public float time;

    public List<WorldObjectAction> prerequisiteActions = new List<WorldObjectAction>();

    public bool IsAvailable(Player player)
    {
        return player.ActionAvailable(this);
    }

    public void Perform()
    {

        switch (actionType)
        {
            case ActionType.Build:
                Build();
                break;
            case ActionType.Recruit:
                Recruit();
                break;
            case ActionType.Science:
                Science();
                break;
            default:
                break;
        }
    }

    private void Science()
    {
        // bij de player opslaan. Da's alles. Deze maakt mogelijk dat andere acties kunnen.
    }

    private void Recruit()
    {
        // instantiate het object en laat lopen naar een rally point
    }

    private void Build()
    {
        // activeer een building placement
    }
}
