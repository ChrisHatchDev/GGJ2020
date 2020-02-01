using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BW_StayUpright : MonoBehaviour
{
    private GameObject targetObject;
    public 

     void Start()
    {
        targetObject = gameObject; 
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        // Smoothly rotates towards target 
        var targetPoint = targetObject.transform.position;
        var targetRotation = Quaternion.LookRotation(targetPoint - transform.position, Vector3.up);
        transform.rotation = targetRotation;
    }
}
