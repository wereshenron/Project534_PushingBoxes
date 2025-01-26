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
    public float baseSpeed;
    public float gravity;
    public float jumpForce;
    public float sprintBoost;
    public float groundedDetection;
    public float rotationSpeed;
    public float speed;
    public float drag;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private bool isGrounded = false;
    private bool jumpRequested = false;
    private bool _isRagdoll = false;
    private bool _isHolding = false;
    private float vertical;
    private float horizontal;
    private HeadPusher _headPusher;
    [SerializeField] private Camera _camera;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        if (!_isRagdoll)
        {
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        }
        else
        {
            _rigidbody.constraints = RigidbodyConstraints.None;
        }

        _animator = GetComponent<Animator>();

        if (_camera == null)
        {
            _camera = Camera.main;
        }

        HeadPusher headPusher = gameObject.GetComponentInChildren<HeadPusher>();
        if (headPusher)
        {
            _headPusher = headPusher;
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

        // Is ragdollin 
        if (Input.GetKeyDown(KeyCode.F) && !_isRagdoll)
        {

            if (_rigidbody.velocity.magnitude > 0.1f)
            {
                _rigidbody.constraints = RigidbodyConstraints.None;
                _rigidbody.drag = 0;
                _isRagdoll = true;
                return;
            }

            if (!_headPusher)
            {
                return;
            }
            StartRagdoll();
        }
        else if (Input.GetKeyDown(KeyCode.F) && _isRagdoll && isGrounded)
        {
            // Stand back up. Includes resetting RB constraints
            StartCoroutine(RotateToUpright());
            _rigidbody.drag = drag;
            _isRagdoll = false;
        }

        // Handle Pickup/Letgo input
        if (TryGetComponent<Pickup>(out var pickup))
        {
            if (Input.GetKeyDown(KeyCode.E) && !pickup.isHolding)
            {
                if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, pickup.detectionRange, pickup.layerMask))
                {
                    Debug.Log("hitting");
                    Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                    pickup.AttemptPickup(rb);
                }
            }

            // Saving for later - THIS IS HOW YOU CONTROL OBJECT YOU CLICK ON   
            // if (Input.GetKeyDown(KeyCode.E) && !pickup.isHolding)
            // {
            //     if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, pickup.detectionRange, pickup.layerMask))
            //     {
            //         Debug.Log("hitting");
            //         _rigidbody = hit.collider.GetComponent<Rigidbody>();
            //         pickup.AttemptPickup(_rigidbody);
            //     }
            // }
            else if (Input.GetKeyDown(KeyCode.E) && pickup.isHolding)
            {
                pickup.AttemptRelease();
            }
        }

        // Handle jump input
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            jumpRequested = true;
        }

        // Update animator Speed
        _animator.SetFloat("Speed", _rigidbody.velocity.magnitude);
    }

    void FixedUpdate()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        Vector3 moveDirection = (_camera.transform.forward * vertical + _camera.transform.right * horizontal).normalized;
        moveDirection.y = 0;
        if (!_isRagdoll)
        {
            // _rigidbody.velocity = new Vector3(moveDirection.x * speed, _rigidbody.velocity.y, moveDirection.z * speed);
            _rigidbody.AddForce(new Vector3(moveDirection.x * speed, moveDirection.y, moveDirection.z * speed), ForceMode.Force);

            if (moveDirection != Vector3.zero)
            {
                // Rotate Character towards direction of movement
                Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
            }
        }


        // Handle Jump
        if (jumpRequested)
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            // isGrounded = false;
            jumpRequested = false;
            StartCoroutine(PushHeadOnDelay());
        }
    }

    void StartRagdoll()
    {
        if (_headPusher == null || _rigidbody == null)
        {
            return;
        }

        _rigidbody.constraints = RigidbodyConstraints.None;
        _isRagdoll = true;
        _rigidbody.drag = 0;
        _headPusher.PushDatHead(_rigidbody);
    }

    bool UpdateIsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundedDetection);
    }

    // Coroutines
    private IEnumerator RotateToUpright()
    {
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
        float elapsedTime = 0f;
        float rotationDuration = 0.5f;

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / rotationDuration;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);
            yield return null;  // Wait for the next frame
        }

        transform.rotation = targetRotation;  // Ensure we snap exactly to target at the end
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private IEnumerator PushHeadOnDelay()
    {
        float elapsedTime = 0f;
        float rotationDuration = 0.2f;

        while (elapsedTime < rotationDuration)
        {

            elapsedTime += Time.deltaTime;
            yield return null;  // Wait for the next frame
        }

        StartRagdoll();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }
}
