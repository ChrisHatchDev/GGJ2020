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
    public float PlayerDistanceThreshold = 1;

    private List<Vector3> spawnLocations = new List<Vector3>();

    public int SmallSpawnAmount = 1;
    public List<GameObject> SmallProps = new List<GameObject>();
    public int MediumSpawnAmount = 1;
    public List<GameObject> MediumProps = new List<GameObject>();
    public int TreeSpawnAmount = 1;
    public List<GameObject> Trees = new List<GameObject>();
    public int BuildingSpawnAmount = 1;
    public List<GameObject> Buildings = new List<GameObject>();

    public GameObject Pig;
    public GameObject Meep;

    public GameObject CreatureToSpawn;

    public CH_TerrainPainting TerrainPainter;
    public CH_CameraPosition CameraController;
    public GameObject TrailParticles;


    void Start()
    {
        previousSpawnLocation = transform.position;
        TrailParticles.SetActive(false);
        SelectTreeTerrain();
    }


    void SpawnObjectsSequence () 
    {
        Debug.Log("Spawn Object");

        previousSpawnLocation = transform.position;
        spawnLocations.Add(transform.position);

        if(CreatureToSpawn != null){
            SpawnAnimals();
        }

        for (int i = 0; i < SmallSpawnAmount; i++)
        {
            StartCoroutine(SpawnObject(SmallProps[Random.Range(0, SmallProps.Count)], true, false));
        }

        for (int i = 0; i < MediumSpawnAmount; i++)
        {
            StartCoroutine(SpawnObject(MediumProps[Random.Range(0, MediumProps.Count)], true, false));
        }

        for (int i = 0; i < TreeSpawnAmount; i++)
        {
            // Spawn a tree only so often
            StartCoroutine(SpawnObject(Trees[Random.Range(0, Trees.Count)], true, true));
        }

        for (int i = 0; i < BuildingSpawnAmount; i++)
        {
            // Spawn a tree only so often
            StartCoroutine(SpawnObject(Buildings[Random.Range(0, Buildings.Count)], false, false));
        }


    }

    IEnumerator SpawnObject (GameObject objectToSpawn, bool matchGroundLevel, bool randomScale) {

        if (objectToSpawn == null) {
            Debug.LogError("Prop List was Empty");
            yield break;
        }

        // yield return new WaitForSeconds(Random.Range(0, 0.2f));

        float spawnFallOff = SpawnDistanceThreshold * 0.25f;

        Vector3 rayPoint = new Vector3(
            Random.Range((transform.position.x - SpawnDistanceThreshold) - spawnFallOff, (transform.position.x + SpawnDistanceThreshold) + spawnFallOff),
            transform.position.y,
             Random.Range((transform.position.z - SpawnDistanceThreshold) - spawnFallOff, (transform.position.z + SpawnDistanceThreshold) + spawnFallOff));

            /*
            Random.Range((transform.position.z - PlayerDistanceThreshold) - SpawnDistanceThreshold, (transform.position.z + PlayerDistanceThreshold) + spawnFallOff) */
        if(CanPropSpawnHere(rayPoint) == false) {
            yield break;
        }

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

                if(matchGroundLevel) {
                    newProp.transform.rotation = Quaternion.Euler(
                        downHit.normal.x,
                        Random.Range(0,360),
                        downHit.normal.y
                    );
                } else {
                    newProp.transform.rotation = Quaternion.Euler(
                        newProp.transform.rotation.x,
                        Random.Range(0,360),
                        newProp.transform.rotation.z
                    );
                }

                if(randomScale) {
                    StartCoroutine(ScaleObject(newProp, Random.Range(0.5f, 1.4f)));
                } else {
                    StartCoroutine(ScaleObject(newProp, -1));
                }
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

    bool CanPropSpawnHere (Vector3 potentialPropSpawnPoint) {

        foreach(Vector3 loc in spawnLocations) {

            if (Vector3.Distance(potentialPropSpawnPoint, loc) < SpawnDistanceThreshold) {
                Debug.Log("CANT Spawn");
                return false;
            }

        }

        Debug.Log("Can Spawn");
        return true;
    }

    void SpawnAnimals() {
        
        for (int i = 0; i < Random.Range(0,2); i++)
        {
            StartCoroutine(SpawnObject(CreatureToSpawn, true, false));
        }
    }

    void GetEcoSystemFromHeight () {

        foreach(Vector3 layer in TerrainLayers.heightLayers){ 
            if(transform.position.y >= layer.x && transform.position.y < layer.y) {

                Debug.Log("Terrain layer = " + layer);
                Debug.Log("Terrain layer index = " + TerrainLayers.heightLayers.IndexOf(layer));

            }
        }

    }

    IEnumerator ScaleObject (GameObject targetObj, float targetScale) {

        while(true) {
            
            float growthRate = GrowthTimeCurve.Evaluate(GrowthSpeed);
            // float scaleCurve = GrowthSizeCurve.Evaluate(GrowthSpeed * Time.deltaTime);

            if(targetScale != -1) {
                targetObj.transform.localScale = Vector3.LerpUnclamped(targetObj.transform.localScale, new Vector3(targetScale,targetScale,targetScale), growthRate);
            } else {
                targetObj.transform.localScale = Vector3.LerpUnclamped(targetObj.transform.localScale, Vector3.one, growthRate);
            }

            if(targetObj.transform.localScale.y == Vector3.one.y) {
                break;
            }   

            yield return null;
        }

    }

    void SelectRockyTerrain () {
        SmallSpawnAmount = 2;
        MediumSpawnAmount = 4;
        TreeSpawnAmount = 1;
        BuildingSpawnAmount = 0;
        
        TerrainPainter.SetTextureParams(1,0);

        CreatureToSpawn = null;
    }

    void SelectTreeTerrain () {
        SmallSpawnAmount = 2;
        MediumSpawnAmount = 3;
        TreeSpawnAmount = 3;
        BuildingSpawnAmount = 0;
        
        TerrainPainter.SetTextureParams(1,1);

        CreatureToSpawn = Pig;
    }

    void SelectGrassLands () {
        SmallSpawnAmount = 3;
        MediumSpawnAmount = 1;
        TreeSpawnAmount = 0;
        BuildingSpawnAmount = 0;

        TerrainPainter.SetTextureParams(1,1);

        CreatureToSpawn = Pig;
    }

    void SelectHouses () {
        SmallSpawnAmount = 2;
        MediumSpawnAmount = 1;
        TreeSpawnAmount = 0;
        BuildingSpawnAmount = 1;

        TerrainPainter.SetTextureParams(1,2);

        CreatureToSpawn = Meep;
    }


    void Update()
    {

        if(Input.GetKey(KeyCode.Space)) {
            CameraController.CloseMode = false;
            TrailParticles.SetActive(true);
            if(Vector3.Distance(previousSpawnLocation, transform.position) > SpawnDistanceThreshold) {
                if(CanSpawnHere() == true) {
                    SpawnObjectsSequence();
                }
            }
        } else { 
            CameraController.CloseMode = true;
        }

        if(Input.GetKeyDown(KeyCode.F)) {
            SelectGrassLands();
        }

        if(Input.GetKeyDown(KeyCode.G)) {
            SelectTreeTerrain();
        }

        if(Input.GetKeyDown(KeyCode.H)) {
            SelectRockyTerrain();
        }

        if(Input.GetKeyDown(KeyCode.J)) {
            SelectHouses();
        }
        
    }
}
