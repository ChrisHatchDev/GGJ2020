using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

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
    public int GrassSpawnAmount = 1;
    public List<GameObject> GrassProps = new List<GameObject>();
    public int BuildingSpawnAmount = 1;
    public List<GameObject> Buildings = new List<GameObject>();

    public GameObject Pig;
    public GameObject Meep;

    public GameObject CreatureToSpawn;

    public CH_TerrainPainting TerrainPainter;
    public CH_CameraPosition CameraController;
    public ParticleSystem TrailParticles;

    private Player player; // The Rewired Player

    List<bool> FaceButtons = new List<bool>();

    void Start()
    {
        previousSpawnLocation = transform.position;
        TrailParticles.Pause();
        SelectTreeTerrain();

        FaceButtons.Add(false);
        FaceButtons.Add(false);
        FaceButtons.Add(false);
        FaceButtons.Add(false);

        player = ReInput.players.GetPlayer(0);
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

        for (int i = 0; i < GrassSpawnAmount; i++)
        {
            // Spawn a tree only so often
            StartCoroutine(SpawnObject(GrassProps[Random.Range(0, GrassProps.Count)], false, false));
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
        
        for (int i = 0; i < Random.Range(1,3); i++)
        {
            StartCoroutine(SpawnObject(CreatureToSpawn, false, false));
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
        GrassSpawnAmount = 1;
        TerrainPainter.SetTextureParams(1,0);

        CreatureToSpawn = null;
    }

    void SelectTreeTerrain () {
        SmallSpawnAmount = 2;
        MediumSpawnAmount = 3;
        TreeSpawnAmount = 4;
        BuildingSpawnAmount = 0;
        GrassSpawnAmount = 8;

        TerrainPainter.SetTextureParams(1,1);

        CreatureToSpawn = Pig;
    }

    void SelectGrassLands () {
        SmallSpawnAmount = 3;
        MediumSpawnAmount = 1;
        TreeSpawnAmount = 0;
        BuildingSpawnAmount = 0;
        GrassSpawnAmount = 8;

        TerrainPainter.SetTextureParams(1,1);

        CreatureToSpawn = Pig;
    }

    void SelectHouses () {
        SmallSpawnAmount = 2;
        MediumSpawnAmount = 1;
        TreeSpawnAmount = 0;
        BuildingSpawnAmount = 1;
        GrassSpawnAmount = 5;
        TerrainPainter.SetTextureParams(1,2);

        CreatureToSpawn = Meep;
    }


    void Update()
    {

        bool ShouldSpawn =false;

        FaceButtons[0] = player.GetButton("Biome1");
        FaceButtons[1] = player.GetButton("Biome2");
        FaceButtons[2] = player.GetButton("Biome3");
        FaceButtons[3] = player.GetButton("Biome4");

        foreach(bool button in FaceButtons) {
            if(button){
                ShouldSpawn = true;
            }
        }

        if(player.GetButton("Biome1")) {
            CameraController.CloseMode = false;
            SelectGrassLands();
        }

        if(player.GetButton("Biome2")) {
            CameraController.CloseMode = false;
            SelectRockyTerrain();
        }

        if(player.GetButton("Biome3")) {
            CameraController.CloseMode = false;
            SelectTreeTerrain();
        }

        if(player.GetButton("Biome4")) {
            CameraController.CloseMode = false;
            SelectHouses();
        }

        if(ShouldSpawn) {
            CameraController.CloseMode = false;
            TrailParticles.Play();

            if(Vector3.Distance(previousSpawnLocation, transform.position) > SpawnDistanceThreshold) {
                if(CanSpawnHere() == true) {
                    SpawnObjectsSequence();
                }
            }
        } else { 
            TrailParticles.Pause();
            CameraController.CloseMode = true;
        }
        
    }
}
