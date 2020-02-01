using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BW_CameraPosition : MonoBehaviour   
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public Vector3 farOffset;

    public BW_PlayerControler_v2 playerController;

    // Update is called once per frame
    void FixedUpdate()
    {

        // Debug.Log("Magnitude: " + playerController.rb.velocity.magnitude);

        float speedScale = 0; 
        speedScale = Mathf.InverseLerp(8, 20, playerController.rb.velocity.magnitude);

        // Debug.Log("Speed scale = " + speedScale);

        Vector3 desiredPosition = target.position + Vector3.Lerp(offset, farOffset, speedScale);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
