using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class GridCrawler : MonoBehaviour
{
    Stopwatch Timer = new Stopwatch();
    bool calculateTotalTime = true;

    public GameObject crawler;
    int width = 500;
    int depth = 500;
    int crawlCount = 100;
    Vector3Int crawlerPos;

    // Start is called before the first frame update
    void Start()
    {
      
       
    }

    // Update is called once per frame
    void Update()
    {
        if (crawlCount > 0)
        {
            Crawl();
            crawlCount--;
        }
        else if (crawlCount == 0 && calculateTotalTime)
        {
            UnityEngine.Debug.Log("Crawler time taken: " + Timer.Elapsed);
            UnityEngine.Debug.Log("Crawler time taken: " + Timer.ElapsedMilliseconds);
            calculateTotalTime = false;
        }
    }

    void Crawl()
    {
        Timer.Start();

        int dx = Random.Range(-1, 2);
        int dz = Random.Range(-1, 2);

        if (Random.Range(0, 2) == 0)
        {
            if (crawlerPos.z + dz * 20 > depth || crawlerPos.z + dz * 20 < 0) dz *= -1;
            crawlerPos += new Vector3Int(0, 0, dz * 20);
        }
        else
        {
            if (crawlerPos.x + dx * 20 > width || crawlerPos.x + dx * 20 < 0) dx *= -1;
            crawlerPos += new Vector3Int(dx * 20, 0, 0);
        }

        crawler.transform.position = crawlerPos;

        Timer.Stop();
    }
}
