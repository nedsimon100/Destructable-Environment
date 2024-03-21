using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMovement : MonoBehaviour
{
    [Header("Camera Controlls")]
    public float xSensitivity = 100f;
    public float ySensitivity = 100f;

    [Header("Movement Controlls")]
    public float ForwardSpeed = 1000f;
    public float strafeSpeed = 800f;
    public float MoveSpeed = 5f;
    public float MaxSpeed = 10f;
    [Header("connected Objects")]
    public GameObject PlayerObject;
    [Header("Jump Controlls")]
    public float jumpForce = 10f;


    private Vector2 MoveDirection;
    private Rigidbody rb;

    private float xRotation, yRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
        movePlayerInputs();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }
    private void FixedUpdate()
    {
        movePlayerOutputs();
    }
    public void movePlayerOutputs()
    {

        rb.AddForce(((PlayerObject.transform.forward * MoveDirection.y) + (PlayerObject.transform.right * MoveDirection.x)).normalized * MoveSpeed, ForceMode.Force);
        Vector3.ClampMagnitude(rb.velocity, MaxSpeed);
    }
    public void movePlayerInputs()
    {
        MoveDirection = new Vector2(Input.GetAxisRaw("Horizontal") * strafeSpeed, Input.GetAxisRaw("Vertical") * ForwardSpeed);

    }

    public void RotateCamera()
    {
        Vector2 mouseMove = new Vector2(Input.GetAxisRaw("Mouse X") * xSensitivity*Time.deltaTime, Input.GetAxisRaw("Mouse Y") * ySensitivity * Time.deltaTime);

        yRotation += mouseMove.x;
        xRotation -= mouseMove.y;

        Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        PlayerObject.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    public void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(UnityEngine.Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    private bool IsGrounded()
    {
        float distanceToGround = GetComponent<Collider>().bounds.extents.y + 1.0f;
        return Physics.Raycast(transform.position, UnityEngine.Vector3.down, distanceToGround);
    }
}
