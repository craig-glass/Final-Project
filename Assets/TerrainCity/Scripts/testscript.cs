using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscript : MonoBehaviour
{
    List<Vector2> roadpoints;
    Dictionary<Vector2, List<Vector2>> roadLocations;

    // Start is called before the first frame update
    void Start()
    {
        Test();
        foreach (KeyValuePair<Vector2, List<Vector2>> t in roadLocations)
        {

            UnityEngine.Debug.Log("key: " + t.Key);
            for (int i = 0; i < t.Value.Count; i++)
            {
                UnityEngine.Debug.Log("value: " + t.Value[i]);
            }
        }

        //int roadpointscount = 0;
        //while (roadpointscount < roadpoints.Count)
        //{
        //    for (int i = 0; i < roadpoints[roadpointscount].Count; i++)
        //    {
        //        UnityEngine.Debug.Log("roadpoints[" + roadpointscount + "][" + i + "] = " + roadpoints[roadpointscount][i]);
        //    }
        //    roadpointscount++;
        //}

    }

    // Update is called once per frame
    void Update()
    {

    }



    void Test()
    {
        
        roadLocations = new Dictionary<Vector2, List<Vector2>>();

        for (int x = 0; x < 3; x++)
        {
            
            for (int z = 0; z < 3; z++)
            {
                if (x == 0 || x == 2)
                {
                    roadpoints = new List<Vector2>();
                    roadpoints.Add(new Vector2(11,22));
                }
                else
                {
                    roadpoints = new List<Vector2>();
                    roadpoints.Add(new Vector2(x, z));
                }

                
            }
            roadLocations.Add(new Vector2(x, 0), roadpoints);
        }
        
    }
}
