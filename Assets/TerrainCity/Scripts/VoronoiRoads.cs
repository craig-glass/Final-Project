using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class VoronoiRoads
{
    public static int[,] voronoiMap = null;
    public static Vector2 direction;
    public static List<Quaternion> q;
    public static Dictionary<Vector2, List<Vector2>> roadLocations;
    public static List<Vector2> testlist;
    public static Dictionary<Vector2, Quaternion> edges;


    public static Dictionary<int, Vector2Int> locations = new Dictionary<int, Vector2Int>();
    public static void GenerateVoronoi(int numDistricts, int startPos, int width, int height)
    {
        voronoiMap = new int[width, height];

        roadLocations = new Dictionary<Vector2, List<Vector2>>();
        q = new List<Quaternion>();

        List<Vector2> roads = new List<Vector2>();

        Terrain terrain = Terrain.activeTerrain;

        int i = 1;

        while (locations.Count < numDistricts)
        {
            int x = Random.Range(startPos, width);
            int y = Random.Range(startPos, height);

            if (!locations.ContainsValue(new Vector2Int(x, y)))
            {
                locations.Add(i, new Vector2Int(x, y));
                i++;
            }

        }



        int roadPlacement = 0;
        bool firstIteration = true;

        for (int y = startPos; y < height; y++)
        {
            bool movedUp = true;
            for (int x = startPos; x < width; x++)
            {
                float distance = Mathf.Infinity;

                // set all edge coordinates to 0
                if (x == startPos || y == startPos || x == width - 1 || y == height - 1)
                {
                    voronoiMap[x, y] = 0;
                    continue;
                }

                // set coordinate to key of closest voronoi point
                foreach (KeyValuePair<int, Vector2Int> val in locations)
                {
                    float distanceTo = Vector2Int.Distance(val.Value, new Vector2Int(x, y));

                    if (distanceTo < distance)
                    {
                        voronoiMap[x, y] = val.Key;
                        distance = distanceTo;

                    }
                }

                // if iteration moves up to next row on y axis, reset roadPlacement to key at that point
                if (movedUp && voronoiMap[x - 1, y] == 0)
                {
                    roadPlacement = voronoiMap[x, y];
                    movedUp = false;
                }

                
                // if key of current coordinate is different from the one before, or, different from the one below, then add that coordinate in the dictionary as a place where a road piece will be instantiated
                if (roadPlacement != voronoiMap[x, y] || roadPlacement != voronoiMap[x, y - 1] && voronoiMap[x, y - 1] != 0)
                {
                    if (firstIteration && roadPlacement != voronoiMap[x, y])
                    {
                        testlist = new List<Vector2>();
                        edges = new Dictionary<Vector2, Quaternion>();

                        roadLocations.Add(new Vector2(roadPlacement, voronoiMap[x, y]), testlist);

                        direction = locations[roadPlacement] - locations[voronoiMap[x, y]];

                        direction = Vector2.Perpendicular(direction);
                        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

                        q.Add(Quaternion.Euler(0f, angle, 0f));
                        firstIteration = false;
                    }
                  
                    if (!roadLocations.ContainsKey(new Vector2(roadPlacement, voronoiMap[x, y])) && roadPlacement != voronoiMap[x, y])
                    {
                        testlist = new List<Vector2>();
                        testlist.Add(new Vector2(x, y));
                        
                        roadLocations.Add(new Vector2(roadPlacement, voronoiMap[x, y]), testlist);

                        direction = locations[roadPlacement] - locations[voronoiMap[x, y]];

                        direction = Vector2.Perpendicular(direction);
                        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

                        q.Add(Quaternion.Euler(0f, angle, 0f));

                        

                    }
                    else
                    {
                        if (roadLocations.TryGetValue(new Vector2(roadPlacement, voronoiMap[x, y]), out List<Vector2> value))
                        {
                            value.Add(new Vector2(x, y));
                        }
                        else if (roadLocations.TryGetValue(new Vector2(voronoiMap[x, y], voronoiMap[x, y - 1]), out List<Vector2> othervalue))
                        {
                            othervalue.Add(new Vector2(x, y));
                        }
                        else if (roadLocations.TryGetValue(new Vector2(voronoiMap[x, y - 1], voronoiMap[x, y]), out List<Vector2> thatvalue))
                        {
                            thatvalue.Add(new Vector2(x, y));
                        }

                    }
                
                    roadPlacement = voronoiMap[x, y];


                }
            }
        }
    }
}
