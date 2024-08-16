using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour
{
    public float baseSpeed = 6.0f;
    public float speed = 6.0f;
    public float gravity = -9.8f;
    public float jumpForce = 8.0f;
    public float sprintBoost = 1.4f;
    public float verticalVelocity;
    private CharacterController _characterController;
    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_characterController.isGrounded) {
            verticalVelocity = 0;

            if (Input.GetButtonDown("Jump")) {
                verticalVelocity = jumpForce;
            }
            if (Input.GetKeyDown(KeyCode.LeftShift)) 
            {
                speed *= sprintBoost;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = baseSpeed;
            }
        }
        transform.Translate(0, speed, 0);
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);

        movement = Vector3.ClampMagnitude(movement, speed);
        verticalVelocity += gravity * Time.deltaTime;
        movement.y = verticalVelocity;

        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);
        _characterController.Move(movement);
    }
}
