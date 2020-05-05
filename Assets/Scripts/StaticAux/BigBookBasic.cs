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

}
