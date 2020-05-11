using System;
using System.Collections;
using System.Collections.Generic;
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

    public static WorldObject GetNearestBuildingInList(Vector3 position, List<Building> buildings)
    {
        Building building=null;
        float distance = 99999;
        foreach (Building b in buildings)
        {
            float dist = Vector2.Distance(position, b.transform.position);
            if (dist < distance)
            {
                building = b;
                distance = dist;
            }
        }
        return building;
    }
}
