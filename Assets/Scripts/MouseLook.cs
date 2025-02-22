using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public enum RotationAxes {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }
    
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float horizontalSensitivity = 9.0f;
    public float verticalSensitivity = 9.0f;

    public float minimumVert = -45.0f;
    public float maximumVert = 45.0f;

    private float _rotationX = 0;
    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        if (TryGetComponent<Rigidbody>(out var body)) {
            body.freezeRotation = true;
        }

        _camera = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (axes == RotationAxes.MouseX) {
            transform.Rotate(0, Input.GetAxis("Mouse X") * horizontalSensitivity, 0);
        } else if (axes == RotationAxes.MouseY) {
            _rotationX -= Input.GetAxis("Mouse Y") * verticalSensitivity;
            _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);

            transform.localEulerAngles = new Vector3(_rotationX, transform.localEulerAngles.y, 0);
        } else {
            _rotationX -= Input.GetAxis("Mouse Y") * verticalSensitivity;
            _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);

            float delta = Input.GetAxis("Mouse X") * horizontalSensitivity;
            float rotationY = transform.localEulerAngles.y + delta;

            _camera.transform.localEulerAngles = new Vector3(_rotationX, 0);
            transform.localEulerAngles = new Vector3(0, rotationY, 0);
        }
    }
}
