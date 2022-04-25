using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRoads : MonoBehaviour
{
    public GameObject crossroads;
    public GameObject straightRoad;
    public int width = 5000;
    public int depth = 5000;
    public int startPos = 1000;

    private void Awake()
    {
        Voronoi.GenerateVoronoi(20, startPos + 100, width, depth);
    }
    // Start is called before the first frame update
    void Start()
    {
        CreateGridRoads();
        InstantiateCubes();
    }

    void CreateGridRoads()
    {
        for (int z = startPos; z < depth; z += 200)
        {
            for (int x = startPos; x < width; x += 200)
            {
                Vector3 pos = new Vector3(x, 0, z);
                GameObject r = Instantiate(crossroads, pos, Quaternion.identity);

                pos.z += 100;
                r = Instantiate(straightRoad, pos, Quaternion.identity);
                pos.x += 100;
                pos.z = z;

                r = Instantiate(straightRoad, pos, Quaternion.Euler(0, 90, 0));

            }
        }
    }
    void InstantiateCubes()
    {
        for (int x = startPos + 100; x < depth; x += 200)
        {
            for (int y = startPos + 100; y < width; y += 200)
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.AddComponent<MoveBuilding>();

                Renderer r = go.GetComponent<Renderer>();
                if (Voronoi.voronoiMap[x, y] < 15)
                    r.material.color = Color.green;
                else if (Voronoi.voronoiMap[x, y] < 18)
                    r.material.color = Color.red;
                else if (Voronoi.voronoiMap[x, y] <= 20)
                    r.material.color = Color.blue;

                float perlin = Voronoi.fBM(x * 0.004f, y * 0.004f, 5);

                int h = 1;
                if (perlin < 0.417f) h = 100;
                else if (perlin < 0.509f) h = 200;
                else if (perlin < 0.623f) h = 300;
                else if (perlin < 0.679f) h = 500;
                else h = 600;

                float yScale = h;
                go.transform.localScale = new Vector3(150f, yScale, 150f);
                go.transform.position = new Vector3(x, (yScale / 2) + 100, y);
            }
        }
    }

}
