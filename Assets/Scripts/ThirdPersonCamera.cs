using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;           // Reference to the player
    public Vector3 offset = new Vector3(0, 5, -10); // Camera offset (position behind player)
    public float rotationSpeed = 5f;    // Speed at which the camera rotates

    private float currentRotation = 0f;
    private float currentPitch = 0f;
    private const float MIN_PITCH = -30f;
    private const float MAX_PITCH = 60f;

    void Update()
    {
        // Get input for camera rotation (mouse movement)
        float horizontalInput = Input.GetAxis("Mouse X");
        float verticalInput = Input.GetAxis("Mouse Y");

        // Update camera rotation based on mouse movement
        currentRotation += horizontalInput * rotationSpeed;
        currentPitch -= verticalInput * rotationSpeed;
        currentPitch = Mathf.Clamp(currentPitch, MIN_PITCH, MAX_PITCH);

        // Apply the camera position and rotation
        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        // Set camera position behind the player
        Vector3 desiredPosition = player.position + offset;

        // Rotate the camera around the player (yaw rotation)
        Quaternion rotation = Quaternion.Euler(currentPitch, currentRotation, 0);

        // Apply rotation to the camera's position
        transform.position = player.position + rotation * offset;

        // Make sure the camera is always looking at the player
        transform.LookAt(player.position);
    }
}
