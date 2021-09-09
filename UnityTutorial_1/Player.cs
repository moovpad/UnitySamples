using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool JumpKeyPressed;
    private float HorizontalInput;
    private Rigidbody RigidBodyComponent;
    private int SuperJumpsRemaining;
    [SerializeField] private Transform GroundCheckTransform; 
    [SerializeField] private LayerMask playerMask;

    public void Start()
    {
        RigidBodyComponent = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpKeyPressed = true;
        }

        HorizontalInput = Input.GetAxis("Horizontal") * 4;
    }

    public void FixedUpdate()
    {
        RigidBodyComponent.velocity = new Vector3(HorizontalInput, RigidBodyComponent.velocity.y, 0);

        if (Physics.OverlapSphere(GroundCheckTransform.position, 0.1f, playerMask).Length == 0)
        {
            return;
        }

        if (JumpKeyPressed)
        {
            float jumpPower = 5f;
            if (SuperJumpsRemaining > 0)
            {
                jumpPower *= 2;
                SuperJumpsRemaining--;
            }

            RigidBodyComponent.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
            JumpKeyPressed = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            Destroy(other.gameObject);
            SuperJumpsRemaining++;
        }
    }

}
