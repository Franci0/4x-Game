using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap_Continent : HexMap {

    public override void GenerateMap()
    {
        //First, call the base version to make all hexes we need
        base.GenerateMap();

        //Make some kind of raised area
        ElevateArea(21, 15, 4);

        //Add lumpiness Perlin Noise

        //Set mesh to mountain/hill/flat/water base on height

        //Simulate rainfall/moisture (probably just Perlin it for now) and set plains/grassland plus forests
    }

    void ElevateArea(int q, int r, int radius)
    {
        
    }
}