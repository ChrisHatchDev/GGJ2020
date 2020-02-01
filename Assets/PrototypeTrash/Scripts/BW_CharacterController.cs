using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BW_CharacterController : MonoBehaviour
{
    [HideInInspector]public Rigidbody rb;
    public float speed = 100f;
    public float maxSpeed = 200f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
            print(rb.velocity.magnitude);
        }

            float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);

        rb.AddForce(movement * speed);
    }
}
