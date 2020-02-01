using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{

    public EcoSystemSO TerrainLayers;
    public AnimationCurve GrowthTimeCurve = new AnimationCurve( new Keyframe(0, 0), new Keyframe(1, 1));
    public AnimationCurve GrowthSizeCurve = new AnimationCurve( new Keyframe(0, 0), new Keyframe(1, 1));
    public float GrowthSpeed = 0.5f;

    public Vector3 TargetSpawnLocation;
    RaycastHit downHit;

    private Vector3 previousSpawnLocation;
    public float SpawnDistanceThreshold = 5;

    private List<Vector3> spawnLocations = new List<Vector3>();

    public List<GameObject> SmallProps = new List<GameObject>();
    public List<GameObject> MediumProps = new List<GameObject>();
    public List<GameObject> Trees = new List<GameObject>();


    void Start()
    {
        previousSpawnLocation = transform.position;
    }


    void SpawnObjectsSequence () 
    {
        Debug.Log("Spawn Object");

        previousSpawnLocation = transform.position;
        spawnLocations.Add(transform.position);

        for (int i = 0; i < 2; i++)
        {
            StartCoroutine(SpawnObject(SmallProps[Random.Range(0, SmallProps.Count)]));
        }

        for (int i = 0; i < 2; i++)
        {
            StartCoroutine(SpawnObject(MediumProps[Random.Range(0, MediumProps.Count)]));
        }

        for (int i = 0; i < 1; i++)
        {
            // Spawn a tree only so often
            StartCoroutine(SpawnObject(Trees[Random.Range(0, Trees.Count)]));
        }

    }

    IEnumerator SpawnObject (GameObject objectToSpawn) {

        if (objectToSpawn == null) {
            Debug.LogError("Prop List was Empty");
            yield break;
        }

        // yield return new WaitForSeconds(Random.Range(0, 0.2f));

        float spawnFallOff = SpawnDistanceThreshold * 0.25f;

        Vector3 rayPoint = new Vector3(
            Random.Range(transform.position.x - SpawnDistanceThreshold, transform.position.x + spawnFallOff),
            transform.position.y,
            Random.Range(transform.position.z - SpawnDistanceThreshold, transform.position.z + spawnFallOff) 
        );

        RaycastHit downHit;
        
        if (Physics.Raycast(rayPoint, Vector3.down, out downHit, 4f)) {

            Vector3 spawnLocation = new Vector3(
                Random.Range(downHit.point.x - SpawnDistanceThreshold, downHit.point.x + spawnFallOff),
                downHit.point.y,
                Random.Range(downHit.point.z - SpawnDistanceThreshold, downHit.point.z + spawnFallOff) 
            );

            if (downHit.collider != null){
                GameObject newProp = Instantiate(objectToSpawn, spawnLocation, Quaternion.identity);
                newProp.transform.localScale = Vector3.zero;
                StartCoroutine(ScaleObject(newProp));
            }
        }
    }

    bool CanSpawnHere () {

        foreach(Vector3 loc in spawnLocations) {

            if (Vector3.Distance(transform.position, loc) < SpawnDistanceThreshold) {
                Debug.Log("CANT Spawn");
                return false;
            }

        }

        Debug.Log("Can Spawn");
        return true;
    }

    void GetEcoSystemFromHeight () {

        foreach(Vector3 layer in TerrainLayers.heightLayers){ 
            if(transform.position.y >= layer.x && transform.position.y < layer.y) {

                Debug.Log("Terrain layer = " + layer);
                Debug.Log("Terrain layer index = " + TerrainLayers.heightLayers.IndexOf(layer));

            }
        }

    }

    IEnumerator ScaleObject (GameObject targetObj) {

        while(true) {
            
            float growthRate = GrowthTimeCurve.Evaluate(GrowthSpeed);
            // float scaleCurve = GrowthSizeCurve.Evaluate(GrowthSpeed * Time.deltaTime);

            targetObj.transform.localScale = Vector3.LerpUnclamped(targetObj.transform.localScale, Vector3.one, growthRate);

            if(targetObj.transform.localScale.y == Vector3.one.y) {
                break;
            }   

            yield return null;
        }



    }


    void Update()
    {

        if(Vector3.Distance(previousSpawnLocation, transform.position) > SpawnDistanceThreshold) {
            if(CanSpawnHere() == true) {
                SpawnObjectsSequence();
            }
        }
        
    }
}
