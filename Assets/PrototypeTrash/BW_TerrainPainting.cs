using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BW_TerrainPainting : MonoBehaviour
{
    float[,,] element;
    int mapX, mapY;
    TerrainData terrainData;
    Vector3 terrainPosition;
    public Terrain myTerrain;
    float[,,] map;
    private Vector3 lastPos;

    //Brads Variables
    public int alphaSize = 50;

    void Awake()
    {
        map = new float[myTerrain.terrainData.alphamapWidth, myTerrain.terrainData.alphamapHeight, myTerrain.terrainData.alphamapLayers];

        element = new float[1, 1, myTerrain.terrainData.alphamapLayers];
        terrainData = myTerrain.terrainData;
        terrainPosition = myTerrain.transform.position;

        lastPos = transform.position;
    }

    void Update()
    {
        UpdateMapOnTheTarget();
    }

    void UpdateMapOnTheTarget()
    {
        //just update if you move
        if (Vector3.Distance(transform.position, lastPos) > 1)
        {
            print("paint");
            //convert world coords to terrain coords
            mapX = (int)(((transform.position.x - terrainPosition.x) / terrainData.size.x) * alphaSize/*terrainData.alphamapWidth*/);
            mapY = (int)(((transform.position.z - terrainPosition.z) / terrainData.size.z) * alphaSize/*terrainData.alphamapHeight*/);

            map[mapY, mapX, 0] = element[0, 0, 0] = 0;
            map[mapY, mapX, 1] = element[0, 0, 1] = 1;

            myTerrain.terrainData.SetAlphamaps(mapX, mapY, element);

            lastPos = transform.position;
        }
    }
}
