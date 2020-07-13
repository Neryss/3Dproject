using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController3D : MonoBehaviour
{
    //Movement
    private float x, z;
    public float speed = 10f;
    private float sprintSpeed = 20f;
    public bool isGrounded = false;
    public LayerMask groundLayer;

    //Look and rot
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;

    //Jumping
    private bool jumping;
    private bool readyToJump = true;
    [Range(1, 10)]
    public float jumpVelocity;

    //Assignable
    public Transform camTransform;
    private Rigidbody rb;
    public Transform groundCheckPos;
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
        if(CheckGround())
            Debug.Log("Touching ground");
        else if(CheckGround() == false)
            Debug.Log("Not touching ground");
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
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Move()
    {
        Vector3 baseGravity = new Vector3(0f, rb.velocity.y - .5f, 0f);     //tried with an offset for the gravity
    
        Vector3 direction = new Vector3(x, 0, z);
        Vector3 moveDir = ((transform.forward * direction.z) * speed) + ((transform.right * direction.x) * speed);
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
            Invoke(nameof(ResetJump), 1);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private bool CheckGround()
    {
        if(Physics.Raycast(groundCheckPos.position, Vector3.down, 0.5f))
            return(true);
        else
            return(false);
    }       //doesn't seem to work properly
}
