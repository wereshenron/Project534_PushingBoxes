using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour
{
    public float baseSpeed = 6.0f;
    public float speed = 6.0f;
    public float gravity = -9.8f;
    public float jumpForce = 8.0f;
    public float sprintBoost = 1.4f;

    private Rigidbody _rigidbody;
    private Camera _camera;
    private Animator _animator;
    private bool isGrounded = true;

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
            speed *= sprintBoost;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = baseSpeed;
        }
    }

    void FixedUpdate()
    {
        // Get input for movement
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);

        // Apply gravity if not grounded
        if (!isGrounded)
        {
            movement.y += gravity * Time.fixedDeltaTime;
        }

        // Move Rigidbody
        Vector3 velocity = transform.TransformDirection(movement) * Time.fixedDeltaTime;
        _rigidbody.MovePosition(_rigidbody.position + velocity);

        // Animate 
        float rawSpeed = _rigidbody.velocity.magnitude;
        float normalizedSpeed = Mathf.Clamp(rawSpeed / baseSpeed * sprintBoost, 0f, 1f); // Normalize based on max speed
        _animator.SetFloat("Speed", normalizedSpeed);

        // Handle Jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // Check if grounded
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f) // Surface is mostly flat
            {
                isGrounded = true;
                break;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // If no collisions, assume not grounded
        isGrounded = false;
    }
}
