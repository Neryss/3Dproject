using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController3D : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;
    public Transform camTransform;
    public float speed = 10f;
    private float sprintSpeed = 20f;
    private Rigidbody rb;
    private float saveSpeed;
    public bool isGrounded = false;
    [Range(1, 10)]
    public float jumpVelocity;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        MouseController();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    //Use the fixed update since we calculate physics
    void FixedUpdate()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            Sprint();
        }
        else
        {
            StopSprint();
        }
        Move();
    }

    private void MouseController()
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
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(x, 0, z);
        Vector3 moveDir = ((transform.forward * direction.z) * speed) + ((transform.right * direction.x) * speed);
        rb.velocity = moveDir + baseGravity;    //need to add the other vector to apply gravity properly, need to check if it doesn't fuck with other things
    }

    //Lazy smoothing for sprinting
    private void Sprint()
    {
        if(speed < sprintSpeed)
        {
            speed = speed + 1;
        }
        else
        {
            speed = sprintSpeed;
        }
    }

    private void StopSprint()
    {
        speed = 10f;
    }

    private void Jump()
    {
        rb.velocity = Vector3.up * jumpVelocity;
    }
}
