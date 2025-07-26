using UnityEngine;

public class CharacterControllerInput : MonoBehaviour
{
    private PlayerControlls _controls;
    private Vector2 _movementInput;
    private bool _jumpRequested = false;
    private bool _sprintHeld = false;

    public float jumpForce = 5f;
    public float gravity = -9.81f;
    public float sprintBoost = 1.5f;

    private Vector3 _velocity;
    private bool isGrounded = true; // Replace with your actual grounded check

    void Awake()
    {
        _controls = new PlayerControlls();
        _controls.Player.Sprint.started += ctx => _sprintHeld = true;
        _controls.Player.Sprint.canceled += ctx => _sprintHeld = false;
    }

    void OnEnable() => _controls.Enable();
    void OnDisable() => _controls.Disable();

    void Update()
    {
        // Jump
        if (_jumpRequested && isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            _jumpRequested = false;
        }

        // Apply gravity manually or through a character controller as usual
        _velocity.y += gravity * Time.deltaTime;

        // Sprint
        float moveSpeed = _sprintHeld ? 5f * sprintBoost : 5f;
        Vector3 move = new Vector3(_movementInput.x, 0, _movementInput.y);
        move *= moveSpeed;

        // Combine move + gravity for your character controller / rigidbody
        // CharacterController.Move(move * Time.deltaTime);
        // Or apply _velocity.y through rigidbody

        // Example: applying x/z via _velocity
        _velocity.x = move.x;
        _velocity.z = move.z;

        // Apply _velocity to your character
        // e.g., transform.position += _velocity * Time.deltaTime;
    }
}
