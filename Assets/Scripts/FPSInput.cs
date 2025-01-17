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
    public float speed = 6.0f;

    private Rigidbody _rigidbody;
    [SerializeField] private Camera _camera;
    private Animator _animator;
    private bool isGrounded = false;
    private bool jumpRequested = false;
    private float vertical;
    private float horizontal;
    private bool _isRagdoll = false;
    private Vector3 _inputKey;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (!_isRagdoll)
        {
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        }
        _animator = GetComponent<Animator>();
        if (_camera == null)
        {
            _camera = Camera.main;
        }
    }

    void Update()
    {
        // Update grounded state
        isGrounded = UpdateIsGrounded();

        // Handle Sprint
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed *= sprintBoost;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = baseSpeed;
        }

        if (Input.GetKeyDown(KeyCode.F) && !_isRagdoll)
        {
            _rigidbody.constraints = RigidbodyConstraints.None;
            _isRagdoll = true;
        }
        else if (Input.GetKeyDown(KeyCode.F) && _isRagdoll && isGrounded)
        {
            StartCoroutine(RotateToUpright());
            _isRagdoll = false;
        }

        // Handle jump input
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            jumpRequested = true;
        }

        // _rigidbody.velocity.Normalize();
        // Update animator Speed
        _animator.SetFloat("Speed", _rigidbody.velocity.magnitude);

        if (_rigidbody.velocity.magnitude > 0.1f && isGrounded)
        {
            // RotateCharacter();
        }

        _inputKey = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

    }

    void FixedUpdate()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        Vector3 moveDirection = (_camera.transform.forward * vertical + _camera.transform.right * horizontal).normalized;
        if (!_isRagdoll)
        {
            _rigidbody.velocity = new Vector3(moveDirection.x * speed, _rigidbody.velocity.y, moveDirection.z * speed);
        }

        // Testing new input system Vector3 moveDirection = (_camera.transform.forward * vertical + _camera.transform.right * horizontal) * speed;
        // moveDirection.y = 0;
        // if (!_isRagdoll)
        // {
        //     // _rigidbody.velocity = new Vector3(moveDirection.x * speed, _rigidbody.velocity.y, moveDirection.z * speed);
        //     Debug.Log(moveDirection);
        //     _rigidbody.AddForce(moveDirection, ForceMode.VelocityChange);
        // }


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

    private IEnumerator RotateToUpright()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
        float elapsedTime = 0f;
        float rotationDuration = 0.5f;  // Time in seconds for the full rotation

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / rotationDuration;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);
            yield return null;  // Wait for the next frame
        }
        Debug.Log("done rotating");

        transform.rotation = targetRotation;  // Ensure we snap exactly to target at the end
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }
}
