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
            _velocity.y = -2f;
        }

        // Get input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Move relative to camera's forward
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        _controller.Move(baseSpeed * Time.deltaTime * move);

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // Sprint
        if (Input.GetButtonDown("Sprint") && isGrounded)
        {
            _velocity = new Vector3(_velocity.x * sprintBoost, _velocity.y, _velocity.z * sprintBoost);
        }

        // Apply gravity
        _velocity.y += gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
}
