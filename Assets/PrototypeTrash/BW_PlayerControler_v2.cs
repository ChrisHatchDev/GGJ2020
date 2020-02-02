using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class BW_PlayerControler_v2 : MonoBehaviour
{
    [HideInInspector]public Rigidbody rb;
    public float speed;
    public float turnSpeed;
    public float inAirSpeedMod = 0.2f;
    public float angularDrag = 0.5f;
    public float MaxSpeed = 20;
    [HideInInspector]

    private BW_Suspension vehicleSupspension;
    public MeshRenderer fishRenderer;

    private Player player; // Use for input init
    public float InputDeadZone = 0.2f;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        vehicleSupspension = GetComponentInChildren<BW_Suspension>();

        player = ReInput.players.GetPlayer(0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float speedScale = Mathf.InverseLerp(8, 20, rb.velocity.magnitude);
        // TOo much jank for now fishRenderer.material.SetFloat("_WaggleSpeed", Mathf.Lerp( 1, 5, speedScale));

        //Set the angular drag value
        rb.angularDrag = angularDrag;

        if (vehicleSupspension.IsGrounded == true)
        {

            if (player.GetAxis("MoveForward") > InputDeadZone)
            {
                rb.AddForce(transform.forward * (speed * player.GetAxis("MoveForward")));
            }
            if (player.GetAxis("MoveForward") < -InputDeadZone)
            {
                rb.AddForce(transform.forward * (speed * player.GetAxis("MoveForward")));
            }

            if (player.GetAxis("MoveLeftRight") > InputDeadZone)
            {
                rb.AddTorque(transform.up * (turnSpeed * player.GetAxis("MoveLeftRight")));
            }

            if (player.GetAxis("MoveLeftRight") < -InputDeadZone)
            {
                rb.AddTorque(transform.up * (turnSpeed * player.GetAxis("MoveLeftRight")));
            }
        }

        if (vehicleSupspension.IsGrounded == false)
        {

            if (player.GetAxis("MoveForward") > InputDeadZone)
            {
                rb.AddForce(transform.forward * (speed * inAirSpeedMod) * player.GetAxis("MoveForward"));
            }
            if (player.GetAxis("MoveForward") < -InputDeadZone)
            {
                rb.AddForce(transform.forward * (speed * inAirSpeedMod) * player.GetAxis("MoveForward"));
            }

            if (player.GetAxis("MoveLeftRight") > InputDeadZone)
            {
                rb.AddTorque(transform.up * (turnSpeed * player.GetAxis("MoveLeftRight")));
            }

            if (player.GetAxis("MoveLeftRight") < -InputDeadZone)
            {
                rb.AddTorque(transform.up * (turnSpeed * player.GetAxis("MoveLeftRight")));
            }
        }

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, MaxSpeed);

    }
}
