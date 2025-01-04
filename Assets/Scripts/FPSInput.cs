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
    public float rotationSpeed = 10f;

    private Rigidbody _rigidbody;
    [SerializeField] private Camera _camera;
    private Animator _animator;
    private bool isGrounded = false;
    private bool jumpRequested = false;
    private float _speed = 6.0f;
    private Vector3 _moveDirection;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        _animator = GetComponent<Animator>();
        if (_camera == null)
        {
            _camera = Camera.main;
        }
    }

    void Update()
    {

        float deltaX = Input.GetAxis("Horizontal");
        float deltaZ = Input.GetAxis("Vertical");

        _moveDirection = (_camera.transform.forward * deltaZ) + (_camera.transform.right * deltaX);

        // Update grounded state
        isGrounded = UpdateIsGrounded();

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
        
        _rigidbody.velocity.Normalize();
        // Update animator Speed
        _animator.SetFloat("Speed", _rigidbody.velocity.magnitude);

        Debug.Log(_moveDirection);

        if (_moveDirection.magnitude > 0.1f && isGrounded)
        {
            RotateCharacter();
        }


    }

    void FixedUpdate()
    { 

        Vector3 velocity = _moveDirection * _speed;
        velocity.y = _rigidbody.velocity.y;  // Preserve vertical velocity for jumps/falls
        _rigidbody.velocity = velocity;

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

    void RotateCharacter()
    {
        Vector3 cameraTarget = new(_camera.transform.forward.x, 0, _camera.transform.forward.z);
        Quaternion targetRotation = Quaternion.LookRotation(cameraTarget);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
