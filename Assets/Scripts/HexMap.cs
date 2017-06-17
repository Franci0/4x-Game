using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    public int numRows;
    public int numColumns;

    public GameObject hexPrefab;

    public Mesh meshWater;
    public Mesh meshFlatland;
    public Mesh meshHill;
    public Mesh meshMountain;

    public Material matOcean;
    public Material matPlain;
    public Material matGrassland;
    public Material matMountain;

    //Tiles of height above whatever, is a whatever
    public float heightMountain = 1f;
    public float heightHill = 0.6f;
    public float heightFlat = 0.0f;

    private Hex[,] hexes;
    private Dictionary<Hex, GameObject> hexToGameObjectMap;

    public void Start()
    {
        GenerateMap();
    }

    /// <summary>
    /// Generate the whole map
    /// </summary>
    virtual public void GenerateMap()
    {
        hexes = new Hex[numColumns, numRows];
        hexToGameObjectMap = new Dictionary<Hex, GameObject>();

        //Generate a map fill with ocean
        for (int column = 0; column < numColumns; column++)
        {
            for (int row = 0; row < numRows; row++)
            {
                //Instantiate a new Hex
                Hex h = new Hex(column, row);
                h.elevation = -0.5f;

                hexes[column, row] = h;

                Vector3 pos = h.PositionFromCamera(Camera.main.transform.position, numRows, numColumns);

                GameObject hexGO = Instantiate(hexPrefab, pos, Quaternion.identity, transform);

                hexToGameObjectMap[h] = hexGO;

                hexGO.GetComponent<HexComponent>().hex = h;
                hexGO.GetComponent<HexComponent>().hexMap = this;

                hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", column, row);
            }
        }
        //StaticBatchingUtility.Combine(gameObject);
        UpdateHexVisual();
    }

    public Hex GetHexAt(int x, int z)
    {
        if (hexes == null)
        {
            Debug.LogError("Hexes array not yet instantiated");
            return null;
        }

        x = x % numColumns;
        if (x < 0)
        {
            x += numColumns;
        }

        z = z % numRows;
        if (z < 0)
        {
            z += numRows;
        }

        return hexes[x, z];
    }

    public void UpdateHexVisual()
    {
        for (int column = 0; column < numColumns; column++)
        {
            for (int row = 0; row < numRows; row++)
            {
                Hex hex = hexes[column, row];

                GameObject hexGO = hexToGameObjectMap[hex];

                MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();

                if (hex.elevation >= heightMountain)
                {
                    mr.material = matMountain;
                }
                else if (hex.elevation >= heightHill)
                {
                    mr.material = matGrassland;
                }
                else if (hex.elevation >= heightFlat)
                {
                    mr.material = matPlain;
                }
                else
                {
                    mr.material = matOcean;
                }

                MeshFilter mf = hexGO.GetComponentInChildren<MeshFilter>();
                mf.mesh = meshWater;
            }
        }
    }

    public Hex[] GetHexesWithinRangeOf(Hex centerHex, int range)
    {
        List<Hex> result = new List<Hex>();

        for (int dx = -range; dx < range - 1; dx++)
        {
            for (int dz = Mathf.Max(-range + 1, -dx - range); dz < Mathf.Min(range, -dx + range - 1); dz++)
            {
                result.Add(GetHexAt(centerHex.q + 1 + dx, centerHex.r + dz));
            }
        }

        return result.ToArray();
    }
}
