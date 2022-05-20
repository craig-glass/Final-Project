using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GridSystemOnTerrain : MonoBehaviour
{
    Stopwatch Timer = new Stopwatch();
    public GameObject straight;

    public int width = 620;
    public int depth = 620;

    // Start is called before the first frame update
    void Start()
    {
        Timer.Start();

        for (int z = 0; z < depth; z += 20)
        {
            for (int x = 0; x < width; x += 20)
            {
                Vector3 pos = new Vector3(x, 0, z);
                GameObject r = Instantiate(straight, pos, Quaternion.identity);
             

                //pos.x += 10;
                //pos.z = z;

                //r = Instantiate(straight, pos, Quaternion.Euler(0, 90, 0));
            }
        }
        Timer.Stop();
        UnityEngine.Debug.Log("Time taken grid system: " + Timer.Elapsed);
        UnityEngine.Debug.Log("Time taken grid system: " + Timer.ElapsedMilliseconds);
    }

}
