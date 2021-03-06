﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController3D : MonoBehaviour
{
    //Movement
    private float x, z;
    public float speed = 10f;
    private float speedMult = 1f;
    private float sprintSpeed = 20f;
    public bool isGrounded = false;
    public LayerMask groundLayer;

    //Look and rot
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;

    //Jumping
    private bool jumping;
    private bool readyToJump = true;
    private float jumpCd = 0.25f;
    [Range(1, 10)]
    public float jumpVelocity;

    //Assignable
    private Rigidbody rb;
    public Transform groundCheckPos;
    public Transform cameraHandler;
    public Transform orientation;
    // Start is called before the first frame update

    void Awake() 
    {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = CheckGround();
        MyInput();
        Look();
    }

    private void Look()
    {
        float desiredX;
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //To find the actual look rotation
        Vector3 rot = cameraHandler.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraHandler.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0f);
        //transform.rotation = Quaternion.Euler(transform.rotation.x, desiredX, transform.rotation.z);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    private void Move()
    {
        //Add extra gravity
        Vector3 baseGravity = new Vector3(0f, rb.velocity.y - .5f, 0f);     //tried with an offset for the gravity

        //Adjust speed while airborne
        if(!isGrounded)
            speedMult = 0.5f;
        else
            speedMult = 1f;
        Vector3 direction = new Vector3(x, 0, z);
        Vector3 moveDir = ((orientation.forward * direction.z) * speed * speedMult) + ((orientation.right * direction.x) * speed * speedMult);
        rb.velocity = moveDir + baseGravity;    //need to add the other vector to apply gravity properly, need to check if it doesn't fuck with other things

        if(jumping && readyToJump)
            Jump();
    }

    private void MyInput()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        jumping = Input.GetKey(KeyCode.Space);
    }

    //Lazy smoothing for sprinting
    private void Sprint()
    {
        if(speed < sprintSpeed)
            speed = speed + 1;
        else
            speed = sprintSpeed;
    }

    private void StopSprint()
    {
        speed = 10f;
    }

    private void Jump()
    {
        if(isGrounded && readyToJump)
        {
            readyToJump = false;
            if(rb.velocity.y < 0.5f)
                rb.velocity = Vector3.up * jumpVelocity;
            else if(rb.velocity.y > 0)
                rb.velocity = (Vector3.up * jumpVelocity) / 2;
            Invoke(nameof(ResetJump), jumpCd);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private bool CheckGround()
    {
        if(Physics.Raycast(groundCheckPos.position, Vector3.down, 0.5f, groundLayer))
            return(true);
        else
            return(false);
    }
}
