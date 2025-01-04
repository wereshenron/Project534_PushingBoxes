using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    public float detectionRange = 10f;
    public float rotationSensitivity = 5f;
    public Material outlineMaterial;
    public Transform _player;
    private Camera _camera;
    void Start()
    {
        _camera = GetComponent<Camera>();

        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // if (_player != null)
        // {
        //     Rigidbody rb = _player.GetComponent<Rigidbody>();
        //     if (rb != null && rb.velocity.magnitude > 0.1f)
        //     {
        //         float y = Input.GetAxis("Mouse X");
        //         Vector3 rotate = new(0, y * rotationSensitivity * Time.deltaTime, 0);
        //         _player.eulerAngles += rotate;
        //     }
        // }


    }

    void OnGUI()
    {
        int size = 12;
        float posX = _camera.pixelWidth / 2 - size / 4;
        float posY = _camera.pixelHeight / 2 - size / 2;
        GUI.Label(new Rect(posX, posY, size, size), "*");
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * detectionRange);
    }
}
