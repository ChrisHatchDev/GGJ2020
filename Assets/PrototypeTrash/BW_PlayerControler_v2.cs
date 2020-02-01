using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BW_PlayerControler_v2 : MonoBehaviour
{
    [HideInInspector]public Rigidbody rb;
    public float speed;
    public float turnSpeed;
    public float inAirSpeedMod = 0.2f;
    public float angularDrag = 0.5f;
    [HideInInspector]

    private BW_Suspension vehicleSupspension;


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        vehicleSupspension = GetComponentInChildren<BW_Suspension>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Set the angular drag value
        rb.angularDrag = angularDrag;

        if (vehicleSupspension.IsGrounded == true)
        {

            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(transform.forward * speed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(transform.forward * -speed);
            }

            if (Input.GetKey(KeyCode.D))
            {
                rb.AddTorque(transform.up * turnSpeed);
            }

            if (Input.GetKey(KeyCode.A))
            {
                rb.AddTorque(transform.up * -turnSpeed);
            }
        }

        if (vehicleSupspension.IsGrounded == false)
        {

            if (Input.GetKey(KeyCode.W))
            {
                rb.AddForce(transform.forward * speed * inAirSpeedMod);
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(transform.forward * -speed * inAirSpeedMod);
            }

            if (Input.GetKey(KeyCode.D))
            {
                rb.AddTorque(transform.up * turnSpeed);
            }

            if (Input.GetKey(KeyCode.A))
            {
                rb.AddTorque(transform.up * -turnSpeed);
            }
        }



    }
}
