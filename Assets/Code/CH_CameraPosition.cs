using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CH_CameraPosition : MonoBehaviour   
{
    public Transform target;

    public Transform mimeObject;
    public Transform mimeObjectMedium;
    public Transform mimeObjectFar;

    public float smoothSpeedClose = 0.125f;
    public float smoothSpeedFar = 0.125f;

    public float originalSmoothSpeedClose = 0.125f;
    public float originalSmoothSpeedFar = 0.125f;

    public Vector3 offset;
    public Vector3 farOffset;

    public BW_PlayerControler_v2 playerController;
    public float originalPlayerTurnSpeed;
    public bool CloseMode = true;

    private void Start() {
        originalPlayerTurnSpeed  = playerController.turnSpeed;
        originalSmoothSpeedClose = smoothSpeedClose;
        originalSmoothSpeedFar = smoothSpeedFar;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Debug.Log("Magnitude: " + playerController.rb.velocity.magnitude);

        float speedScale = 0; 
        speedScale = Mathf.InverseLerp(8, 20, playerController.rb.velocity.magnitude);

        Debug.Log("Speed scale = " + speedScale);
        Vector3 desiredPosition = Vector3.one;
        Quaternion desiredRotation;

        if(CloseMode) {

            if(Vector3.Distance(transform.position, mimeObject.transform.position) > 5) {
                smoothSpeedClose = (originalSmoothSpeedClose * 0.25f);
            } else {
                smoothSpeedClose = originalSmoothSpeedClose;
            }

            playerController.turnSpeed = originalPlayerTurnSpeed * 0.6f;
            desiredPosition = Vector3.Lerp(mimeObject.transform.position, mimeObjectMedium.transform.position, speedScale);
            desiredRotation = Quaternion.Lerp(mimeObject.transform.rotation, mimeObjectMedium.transform.rotation, speedScale);
            
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeedClose);
            transform.localRotation = Quaternion.Lerp(transform.rotation, desiredRotation, smoothSpeedClose * 0.8f);
        } else {

            if(Vector3.Distance(transform.position, mimeObjectFar.transform.position) > 5) {
                smoothSpeedFar = (originalSmoothSpeedFar * 0.25f);
            } else {
                smoothSpeedFar = originalSmoothSpeedFar;
            }
            playerController.turnSpeed = originalPlayerTurnSpeed;

            // desiredPosition = Vector3.Lerp(transform.position, mimeObjectFar.transform.position, speedScale);
            // desiredRotation = Quaternion.Lerp(transform.rotation, mimeObjectFar.transform.rotation, speedScale);

            transform.position = Vector3.Lerp(transform.position, mimeObjectFar.transform.position, smoothSpeedFar);
            transform.localRotation = Quaternion.Lerp(transform.rotation, mimeObjectFar.transform.rotation, smoothSpeedFar * 0.8f);
        }

    }
}
