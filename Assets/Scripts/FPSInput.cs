using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using Unity.VisualScripting;
using UnityEngine;

[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour
{
    public float baseSpeed = 6.0f;
    public float gravity = -9.8f;
    public float jumpForce = 8.0f;
    public float sprintBoost = 1.4f;
    public float groundedDetection = 1.0f;
    private bool isGrounded = false;
    private CharacterController _controller;
    private Vector3 _velocity;


    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Ground check
        isGrounded = _controller.isGrounded;
        if (isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f; // Small downward force to stick to the ground
        }

        // Get input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Move relative to camera's forward
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        _controller.Move(move * baseSpeed * Time.deltaTime);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // Apply gravity
        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }


    // void FixedUpdate()
    // {
    //      // Handle Sprint
    //     if (_sprintingBegan)
    //     {
    //         _speed *= sprintBoost;
    //         _sprintingBegan = false;
    //     }
    //     else if (_sprintingEnded)
    //     {
    //         _speed = baseSpeed;
    //         _sprintingEnded = false;
    //     }

    //     // Get input for movement
    //     // Get input for movement
    //     float deltaX = Input.GetAxis("Horizontal");
    //     float deltaZ = Input.GetAxis("Vertical");
    //     Vector3 movement = new Vector3(deltaX, 0, deltaZ);

    //     // Normalize input to prevent faster diagonal movement
    //     if (movement.magnitude > 1)
    //     {
    //         movement.Normalize();
    //     }

    //     movement *= _speed;

    //     // Update animator Speed
    //     _animator.SetFloat("Speed", movement.magnitude);

    //     // Move Rigidbody
    //     Vector3 velocity = transform.TransformDirection(movement) * Time.fixedDeltaTime;
    //     _rigidbody.MovePosition(_rigidbody.position + velocity);

    //     // Handle Jump
    //     if (jumpRequested)
    //     {
    //         _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    //         isGrounded = false;
    //         jumpRequested = false;
    //     }
    // }

    bool UpdateIsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundedDetection);
    }
}
