using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BigBookBasic
{
    public static Vector2 MousePosition()
    {
        Vector3 mousePosition = GameManager.instance.camera.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2(mousePosition.x, mousePosition.y);
    }

    public static T GetComponentFromGameObject<T>(GameObject go) where T : UnityEngine.Object
    {
        //eerst proberen object zelf te pakken
        T comp = go.GetComponent<T>();
        if (comp)
            return comp;
        //anders tot 2 niveaus omhoog de parent.
        else
        {
            Transform par = go.transform.parent;
            if (par)
            {
                go = par.gameObject;
                comp = go.GetComponent<T>();
                if (comp)
                    return comp;
                else
                {
                    par = go.transform.parent;
                    if (par)
                    {
                        go = par.gameObject;
                        comp = go.GetComponent<T>();
                        if (comp)
                            return comp;
                    }
                }
            }
        }
        //anders niks
        return null;
    }

    public static Collider2D PickProminentCollider(Collider2D[] colliders)
    {
        ColliderValue colValue = ColliderValue.Nothing;
        Collider2D prominentCollider = null;

        foreach(Collider2D col in colliders)
        {
            if (col.gameObject.CompareTag("Ground") && colValue == ColliderValue.Nothing)
            {
                prominentCollider = col;
                colValue = ColliderValue.Ground;
                continue;
            }
            if (col.gameObject.CompareTag("Water") && (colValue == ColliderValue.Nothing || colValue==ColliderValue.Ground))
            {
                prominentCollider = col;
                colValue = ColliderValue.Water;
                continue;
            }
            if ((col.gameObject.CompareTag("Character") || col.gameObject.CompareTag("Building")|| (col.gameObject.CompareTag("Resource")) && 
                colValue == ColliderValue.Nothing || colValue == ColliderValue.Ground || colValue == ColliderValue.Water))
            {
                prominentCollider = col;
                break;
            }
        }
        return prominentCollider;
    }

    //public static WorldObject GetNearestWorldObjectInList(Vector3 position, List<Building> buildings)
    //{
    //    Building building=null;
    //    float distance = 99999;
    //    foreach (Building b in buildings)
    //    {
    //        float dist = Vector2.Distance(position, b.transform.position);
    //        if (dist < distance)
    //        {
    //            building = b;
    //            distance = dist;
    //        }
    //    }
    //    return building;
    //}

    public static WorldObject GetNearestWorldObjectInList<T>(Vector3 position, List<T> objects) where T : UnityEngine.MonoBehaviour
    {
        WorldObject wo = null;
        float distance = 99999;
        List<WorldObject> worldObjects = ConvertToWorldObjectList(objects);
        foreach (WorldObject o in worldObjects)
        {
            float dist = Vector2.Distance(position, o.transform.position);
            if (dist < distance)
            {
                wo = o;
                distance = dist;
            }
        }
        return wo;
    }

    private static List<WorldObject> ConvertToWorldObjectList<T>(List<T> objects) where T : UnityEngine.MonoBehaviour
    {
        List<WorldObject> worldObjects = new List<WorldObject>();
        foreach(T o in objects)
        {
            worldObjects.Add(GetComponentFromGameObject<WorldObject>(o.gameObject));
        }
        return worldObjects;
    }

    public static WorldObject GetNearestResourceInRange(Vector3 position, float radius, ResourceType resourceType =ResourceType.None)
    {
        Collider2D[] resources = Physics2D.OverlapCircleAll(position, radius, GameManager.instance.resourcesMask);
        if(resources.Length==0)
            return null;
        List<Resource> resourcesList = new List<Resource>();
        foreach(Collider2D collider in resources)
        {
            Resource resource = GetComponentFromGameObject<Resource>(collider.gameObject);
            if (resource.resourceAmount > 0)
            {
                if (resourceType == ResourceType.None)
                    resourcesList.Add(resource);
                else if (resource.resourceType == resourceType)
                    resourcesList.Add(resource);
            }
        }
        if (resourcesList.Count == 0)
            return null;
        else if (resourcesList.Count == 1)
            return resourcesList.First();
        else
            return GetNearestWorldObjectInList(position, resourcesList);        
    }
}
