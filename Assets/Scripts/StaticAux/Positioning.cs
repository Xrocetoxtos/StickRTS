using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Positioning
{
    public static Vector2[] GetUnitGroupDestinations(Vector2 moveToPos, int numUnits, float unitGap)
    {
        Vector2[] destinations = new Vector2[numUnits];

        int rows = Mathf.RoundToInt(Mathf.Sqrt(numUnits));
        int cols = Mathf.CeilToInt((float)numUnits / (float)rows);

        int curRow = 0;
        int curCol = 0;

        float width = ((float)rows - 1) * unitGap;
        float length = ((float)cols - 1) * unitGap;

        for (int x = 0; x < numUnits; x++)
        {
            destinations[x] = moveToPos + (new Vector2(curRow, curCol) * unitGap) - new Vector2(length / 2, width / 2);
            curCol++;

            if (curCol == rows)
            {
                curCol = 0;
                curRow++;
            }
        }
        return destinations;
    }

    public static Vector2[] GetUnitGroupDestinationsAroundWorldObject(WorldObject worldObject, Vector2[] currentPositions, WorldObject[] worldObjects)
    {
        // kijken welke slots rond de resource walkable zijn.
        Dictionary<Transform, WorldObject> slots = new Dictionary<Transform, WorldObject>();
        for (int s = 0; s < worldObject.slots.Length; s++) 
        {
            if (PointIsWalkable(worldObject.slots[s].position))
                slots.Add(worldObject.slots[s], null);
        }

        if (slots.Count == 0)
            return currentPositions;

        // nu per character een vrije slot zoeken.
        for (int x = 0; x < worldObjects.Length; x++)
        {
            Transform slot = worldObject.transform;
            float distance = 99999;
            foreach (KeyValuePair<Transform,WorldObject> entry in slots)
            {
                if(entry.Value==null)
                {
                    float dist = Vector2.Distance(currentPositions[x], entry.Key.position);
                    if (dist < distance)
                    {
                        slot = entry.Key;
                        distance = dist;
                    }
                }
            }
            if (slot != worldObject.transform)
            {
                slots[slot] = worldObjects[x];
                currentPositions[x] = slot.position;
            }
        }
        return currentPositions;
    }

    public static bool PointIsWalkable(Vector2 point)
    {
        Node node = GameManager.instance.grid.NodeFromWorldPoint(new Vector3(point.x, point.y, 0));
        return node.walkable;
    }

    public static Vector2[] GetCurrentPositions(Character[] characters)
    {
        Vector2[] positions = new Vector2[characters.Length];
        for (int i = 0; i < characters.Length; i++)
        {
            positions[i] = characters[i].transform.position;
        }
        return positions;
    }
}
