using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour
{
    public float baseSpeed = 6.0f;
    public float gravity = -9.8f;
    public float jumpForce = 8.0f;
    public float sprintBoost = 1.4f;
    public float groundedDetection = 1.0f;

    private Rigidbody _rigidbody;
    private Camera _camera;
    private Animator _animator;
    private bool isGrounded = false;
    private bool jumpRequested = false;
    private float _speed = 6.0f;
    private Vector3 _movement;
    private Vector3 _normalizedMovement;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _rigidbody.freezeRotation = true; // Prevent unwanted rotations due to physics
        _camera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        // Handle Sprint
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed *= sprintBoost;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = baseSpeed;
        }

        // Handle jump input
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            jumpRequested = true;
        }

    }

    void FixedUpdate()
    {
        // Get input for movement
        // Get input for movement
        float deltaX = Input.GetAxis("Horizontal");
        float deltaZ = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);

        // Normalize input to prevent faster diagonal movement
        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }

        movement *= _speed;

        // Update animator Speed
        _animator.SetFloat("Speed", movement.magnitude);

        // Move Rigidbody
        Vector3 velocity = transform.TransformDirection(movement) * Time.fixedDeltaTime;
        _rigidbody.MovePosition(_rigidbody.position + velocity);

        // Update grounded state
        isGrounded = UpdateIsGrounded();

        // Handle Jump
        if (jumpRequested)
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            jumpRequested = false;
        }
    }

    bool UpdateIsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundedDetection);
    }
}
