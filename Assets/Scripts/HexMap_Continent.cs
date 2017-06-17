using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap_Continent : HexMap
{

    /*public int hexCoordX;
    public int hexCoordZ;
    public int range;*/

    public override void GenerateMap()
    {
        //First, call the base version to make all hexes we need
        base.GenerateMap();

        int numContinents = 3;
        int continentSpacing = numColumns / numContinents;

        Random.InitState(0);

        for (int c = 0; c < numContinents; c++)
        {
            //Make some kind of raised area
            int numSplats = Random.Range(4, 8);
            for (int i = 0; i < numSplats; i++)
            {
                int range = Random.Range(5, 8);
                int z = Random.Range(range, numRows - range);
                int x = Random.Range(0, 10) - z / 2 + (c * continentSpacing);

                ElevateArea(x, z, range);
            }
        }

        //Add lumpiness Perlin Noise
        float noiseResolution = 0.01f;
        Vector2 noiseOffset = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
        float noiseScale = 2f;  //Larger values makes more islands and lakes
        for (int column = 0; column < numColumns; column++)
        {
            for (int row = 0; row < numRows; row++)
            {
                Hex h = GetHexAt(column, row);
                float n = Mathf.PerlinNoise(((float)column / Mathf.Max(numColumns, numRows) / noiseResolution) + noiseOffset.x,
                    ((float)row / Mathf.Max(numColumns, numRows) / noiseResolution) + noiseOffset.y)
                    - 0.5f;
                h.elevation += n * noiseScale;
            }
        }

        //Set mesh to mountain/hill/flat/water base on height

        //Simulate rainfall/moisture (probably just Perlin it for now) and set plains/grassland plus forests

        //Now make sure all hex visuals are updated to match the data

        UpdateHexVisual();
    }

    void ElevateArea(int q, int r, int range, float centerHeight = 0.8f)
    {
        Hex centerHex = GetHexAt(q, r);

        Hex[] areaHexes = GetHexesWithinRangeOf(centerHex, range);

        foreach (Hex h in areaHexes)
        {
            h.elevation = centerHeight * Mathf.Lerp(1f, 0.25f, Mathf.Pow(Hex.Distance(centerHex, h) / range, 2f));
        }
    }
}