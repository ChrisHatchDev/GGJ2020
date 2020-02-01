using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{

    public GameObject TestProp;
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
    public List<GameObject> LargeProps = new List<GameObject>();
    public List<GameObject> Trees = new List<GameObject>();


    void Start()
    {
        SpawnObjects();
        previousSpawnLocation = transform.position;
    }


    void SpawnObjects () 
    {
        Debug.Log("Spawn Object");

        if(CanSpawnHere() == false) {
            return;
        }

        previousSpawnLocation = transform.position;
        spawnLocations.Add(transform.position);

        RaycastHit downHit;
        
        if (Physics.Raycast(transform.position, Vector3.down, out downHit, 4f)) {
            Debug.Log("Hit something down");
            if (downHit.collider != null){
                if(TestProp != null) {
                    GameObject newProp = Instantiate(TestProp, downHit.point, Quaternion.identity);
                    newProp.transform.localScale = Vector3.zero;
                    StartCoroutine(ScaleObject(newProp));
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
            SpawnObjects();
        }
        
    }
}
