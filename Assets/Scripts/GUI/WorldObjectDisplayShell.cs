using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectDisplayShell : MonoBehaviour
{
    [SerializeField] private GameObject characterDisplay;
    [SerializeField] private GameObject resourceDisplay;
    [SerializeField] private GameObject buildingdisplay;

    public void Setup(WorldObject _worldObject)
    {
        if(_worldObject)
        {

            switch (_worldObject.worldObjectType)
            {
                case ObjectType.Character:
                    SetDisplay(characterDisplay, _worldObject);
                    break;
                case ObjectType.Resource:
                    _worldObject.TryGetComponent<Resource>(out Resource resource);
                    if (resource)
                    {
                        if (resource.available)
                            SetDisplay(resourceDisplay, _worldObject);
                        else
                            SetDisplay(characterDisplay, _worldObject);
                    }
                    else
                    {
                        SetDisplay(null, null);
                    }
                    break;
                case ObjectType.Building:
                    SetDisplay(buildingdisplay, _worldObject);
                    break;
                default:
                    SetDisplay(null, null);
                    break;
            }
        }
    }

    private void SetDisplay(GameObject display, WorldObject _worldObject)
    {
        characterDisplay.SetActive(false);
        resourceDisplay.SetActive(false);
        buildingdisplay.SetActive(false);

        if (display)
        {
            display.SetActive(true);
            display.GetComponent<WorldObjectDisplay>().Setup(_worldObject);
        }
    }
}
