using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherController : MonoBehaviour
{
    private AIController ac;
    private SenseController sense;
    private MovementController movement;
    private Unit unit;

    public List<ResourceType> canExtractResourceType = new List<ResourceType>();

    public ResourceType hasResourceType;
    public float hasResourceAmount;
    public float maxResourceAmount = 10;
    public float resourceExtractSpeed = 1;


    private void Awake()
    {
        ac = GetComponent<AIController>();
        movement = GetComponent<MovementController>();
        sense = GetComponent<SenseController>();
        unit = GetComponent<Unit>();
    }


    public void FindStorage()
    {
        //een storage zoeken om de resources heen te sturen. kunnen álle storages van deze player zijn.
        ac.returnTarget = ac.actualTarget;
        ac.actualTarget = sense.FindStorage(hasResourceType);
        if (ac.actualTarget != null)
        {
            MovementController movement = unit.GetComponent<MovementController>();
            if (movement != null)
            {
                MovementController[] unitArray = BigBookBasic.ThisUnitToArray(movement);
                unit.player.playerController.MoveToObject(ac.actualTarget.gameObject, unitArray);
            }
        }
    }

    public void FindNewResource()
    {
        //als een resource leeg is en character heeft geen resources, dan zoeken naar een nieuwe. moet binnen viewRange.
        ac.returnTarget = null;
        ac.actualTarget = sense.FindNewResource(hasResourceType);
        if (ac.actualTarget != null)
        {
            MovementController movement = unit.GetComponent<MovementController>();
            if (movement != null)
            {
                MovementController[] unitArray = BigBookBasic.ThisUnitToArray(movement);
                unit.player.playerController.MoveToObject(ac.actualTarget.gameObject, unitArray);
                return;
            }
        }
        ac.animator.SetBool("HasResources", false);
        ac.animator.SetBool("IsGathering", false);
    }
}
