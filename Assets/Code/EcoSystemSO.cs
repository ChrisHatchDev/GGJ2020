using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TerrainLayers", menuName = "ScriptableObjects/TerrainLayers", order = 1)]
public class EcoSystemSO : ScriptableObject
{
    public List<string> heightNames = new List<string>();

    // Start and end point of each eco system
    // x= start y= end z=blend distance
    [Tooltip(" Start and end point of each eco system, // x= start y= end z=blend distance")]
    public List<Vector3> heightLayers = new List<Vector3>();
}
