using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Voronoi 
{
    public static int[,] voronoiMap = null;
    public static Dictionary<int, Vector2Int> locations;
    public static void GenerateVoronoi(int numOfPoints, int startPos, int width, int height)
    {
        locations = new Dictionary<int, Vector2Int>();
        voronoiMap = new int[width, height];
        int i = 1;

        while (locations.Count < numOfPoints)
        {
            int x = Random.Range(startPos, width);
            int y = Random.Range(startPos, height);

            if(!locations.ContainsValue(new Vector2Int(x, y)))
            {
                locations.Add(i, new Vector2Int(x, y));
                i++;
                
            }
        }

        

        for (int z = startPos; z < height; z++)
        {
            for (int x = startPos; x < width; x++)
            {
                float distance = Mathf.Infinity;
                foreach (KeyValuePair<int, Vector2Int> location in locations)
                {
                    float distanceTo = Vector2Int.Distance(location.Value, new Vector2Int(x, z));

                    if (distanceTo < distance)
                    {
                        voronoiMap[x, z] = location.Key;
                        distance = distanceTo;
                    }
                }
            }
        }
    }

    public static float fBM(float x, float y, int octaves)
    {
        float total = 0;
        float frequency = 1;
        for (int i = 0; i < octaves; i++)
        {
            total += Mathf.PerlinNoise(x * frequency, y * frequency);
            frequency *= 2;
        }

        return total / (float)octaves;
    }
}
