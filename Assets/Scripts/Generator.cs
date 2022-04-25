using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    Terrain terrain;
    TerrainData terrainData;
    public GameObject voronoiPointRoad;

    // Perlin Noise Params
    public float perlinXScale = 0.0035f;
    public float perlinYScale = 0.0035f;
    public int perlinXOffset = 0;
    public int perlinYOffset = 0;
    public int perlinOctaves = 2;
    public float perlinPersistance = -2.11f;
    public float perlinHeightScale = 0.66f;

    private void Awake()
    {
        Debug.Log("Initializing Terrain Data");
        terrain = GetComponent<Terrain>();
        terrainData = Terrain.activeTerrain.terrainData;

        //Voronoi.GenerateVoronoi(10, 0, 5000, 5000);
        Perlin();
    }
    // Start is called before the first frame update
    void Start()
    {
        //ShowVoronoiPoints(Voronoi.locations);
    }


    void ShowVoronoiPoints(Dictionary<int, Vector2Int> locations)
    {
        foreach (KeyValuePair<int, Vector2Int> location in locations)
        {
            Instantiate(voronoiPointRoad, new Vector3(location.Value.x, 100, location.Value.y), Quaternion.identity);
        }
    }


    public void Perlin()
    {
        float[,] heightMap = GetHeightMap();

        for (int y = 0; y < terrainData.heightmapResolution; y++)
        {
            for (int x = 0; x < terrainData.heightmapResolution; x++)
            {
                heightMap[x, y] += Utils.fBM(x * perlinXScale, y * perlinYScale, perlinOctaves, perlinPersistance, perlinXOffset, perlinYOffset) * perlinHeightScale;
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }

    float[,] GetHeightMap()
    {
        return terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
    }

    private void OnApplicationQuit()
    {
        float[,] resetHeights = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];
        terrainData.SetHeights(0, 0, resetHeights);
    }
}
