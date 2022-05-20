using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBuilding : MonoBehaviour
{
    Terrain terrain;

    private void Awake()
    {
        terrain = Terrain.activeTerrain;
    }
    // Start is called before the first frame update
    void Start()
    {
        Vector3 position = transform.position;
        position.y = terrain.SampleHeight(transform.position);
        position.y += (transform.localScale.y / 2) - (transform.localScale.y / 4);
        transform.position = position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
