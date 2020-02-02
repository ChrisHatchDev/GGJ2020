using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_TerrainPainting : MonoBehaviour
{
    float[,,] element;
    int mapX, mapY;
    TerrainData terrainData;
    Vector3 terrainPosition;
    public Terrain myTerrain;
    float[,,] map;
    float[,,] splatMap;
    private Vector3 lastPos;

    //Brads Variables
    public int alphaSize = 2;

    // Chris Variables
    private int TargetTextureIndex;
    public float TargetTextureStrength = 1;

    public TerrainData OriginalData;

    void Awake()
    {
        map = new float[myTerrain.terrainData.alphamapWidth, myTerrain.terrainData.alphamapHeight, myTerrain.terrainData.alphamapLayers];

        OriginalData = myTerrain.terrainData;


        element = new float[1, 1, myTerrain.terrainData.alphamapLayers];
        terrainData = myTerrain.terrainData;
        terrainPosition = myTerrain.transform.position;

        // splatMap = myTerrain.terrainData.GetAlphamaps(0,0,alphaSize,alphaSize);


        lastPos = transform.position;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Space)) {
            UpdateMapOnTheTarget();
        }
    }

    public void SetTextureParams (int targetLayer, float strength) {

        TargetTextureIndex = targetLayer;
        TargetTextureStrength = strength;

    }

    void UpdateMapOnTheTarget()
    {
        //just update if you move
        if (Vector3.Distance(transform.position, lastPos) > 1)
        {
            print("paint");
            //convert world coords to terrain coords
            mapX = (int)(((transform.position.x - terrainPosition.x) / terrainData.size.x) * terrainData.alphamapWidth);
            mapY = (int)(((transform.position.z - terrainPosition.z) / terrainData.size.z) * terrainData.alphamapHeight);


            Debug.Log("TargetTexture Index: " + TargetTextureIndex);

            // float[,,] splatter = myTerrain.terrainData.GetAlphamaps(mapX, mapY, 10,10);
            // map[mapY, mapX, 0] = element[0, 0, 0] = 0; // Zero sand applied
            map[mapY, mapX, 0] = element[0, 0, 0] = 1; // Half grass applied
            
            myTerrain.terrainData.SetAlphamaps(mapX, mapY, element);

            for (int i = 0; i < 1; i++)
            {   
                myTerrain.terrainData.SetAlphamaps(mapX, mapY + i, element);
                for (int iN = 0; iN < 1; iN++)
                {
                    myTerrain.terrainData.SetAlphamaps(mapX + i, mapY, element);
                }
                for (int iN = 0; iN < 1; iN++)
                {
                    myTerrain.terrainData.SetAlphamaps(mapX - i, mapY, element);
                }
            }

            // myTerrain.terrainData.SetAlphamaps(mapX, mapY, element);

            lastPos = transform.position;
        }
    }

    private void OnApplicationFocus(bool focusStatus) {
        
        if(focusStatus == false) {
            myTerrain.terrainData = OriginalData;
            Debug.Log("Unfocused!");
        }
    }
}
