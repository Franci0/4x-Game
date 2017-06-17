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
            for (int row = 0; row < numRows ; row++)
            {
                //Instantiate a new Hex
                Hex h = new Hex(column, row);

                hexes[column, row] = h;

                Vector3 pos = h.PositionFromCamera(Camera.main.transform.position, numRows, numColumns);

                GameObject hexGO= Instantiate(hexPrefab,pos, Quaternion.identity, transform);

                hexToGameObjectMap[h] = hexGO;

                hexGO.GetComponent<HexComponent>().hex = h;
                hexGO.GetComponent<HexComponent>().hexMap = this;

                hexGO.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", column, row);

                MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
                mr.material = matOcean;

                MeshFilter mf = hexGO.GetComponentInChildren<MeshFilter>();
                mf.mesh = meshWater;
            }
        }
        //StaticBatchingUtility.Combine(gameObject);
    }

    public Hex getHexAt(int x, int z)
    {
        if (hexes == null)
        {
            Debug.LogError("Hexes array not yet instantiated");
            return null;
        }

        return hexes[x % numRows, z % numColumns];
    }
}
