using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Define all the hex transform informations and reference to its neighboors
/// </summary>
public class Hex
{
    //q+r+s=0
    //s=-(q+r)

    public readonly int q; //Column
    public readonly int r; //Row
    public readonly int s;

    //Data for max generation and maybe in-game effects
    public float elevation;
    public float moisture;

    static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;

    float radius = 1f;

    //To improve
    bool allowWrapEastWest = true;
    bool allowWrapNorthSouth = false;

    public Hex(int q, int r)
    {
        this.q = q;
        this.r = r;
        s = -q - r;
    }

    /// <summary>
    /// Return the world-space position of the hex
    /// </summary>
    public Vector3 Position()
    {
        return new Vector3(HexHorizontalSpacing() * (q + r / 2f), 0, HexVerticalSpacing() * r);
    }

    public Vector3 PositionFromCamera(Vector3 cameraPosition, float numRows, float numColumns)
    {
        float mapHeight = numRows * HexVerticalSpacing();
        float mapWidth = numColumns * HexHorizontalSpacing();
        Vector3 position = Position();

        if (allowWrapEastWest)
        {
            float howManyWidthsFromCamera = (position.x - cameraPosition.x) / mapWidth;
            //We want howManyWidthsFromCamera to be between -0.5 to 0.5
            if (howManyWidthsFromCamera > 0)
            {
                howManyWidthsFromCamera += 0.5f;
            }
            else
            {
                howManyWidthsFromCamera -= 0.5f;
            }
            int howManyWidthsToFix = (int)howManyWidthsFromCamera;
            position.x -= howManyWidthsToFix * mapWidth;
        }

        if (allowWrapNorthSouth)
        {
            float howManyHeoightsFromCamera = (position.z - cameraPosition.z) / mapHeight;
            //We want howManyWidthsFromCamera to be between -0.5 to 0.5
            if (howManyHeoightsFromCamera > 0)
            {
                howManyHeoightsFromCamera += 0.5f;
            }
            else
            {
                howManyHeoightsFromCamera -= 0.5f;
            }
            int howManyHeightsToFix = (int)howManyHeoightsFromCamera;
            position.z -= howManyHeightsToFix * mapHeight;
        }

        return position;

    }

    float HexHeight()
    {
        return radius * 2f;
    }

    float HexWidth()
    {
        return WIDTH_MULTIPLIER * HexHeight();
    }

    float HexVerticalSpacing()
    {
        return HexHeight() * 0.75f;
    }

    float HexHorizontalSpacing()
    {
        return HexWidth();
    }
}
