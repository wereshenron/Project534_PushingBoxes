using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    public float detectionRange = 2.0f;

    public float pickupCooldownDuration = 2f;
    public float throwForce = 5f;
    private float _nextPickupTime = 0f;
    private Rigidbody _rigidbody;
    private bool _isHolding = false;
    private Vector3 _previousPosition;
    [SerializeField] private Camera _camera;


    void Start()
    {
        if (_camera != null)
        {
            _camera = GetComponentInChildren<Camera>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_isHolding && Time.time >= _nextPickupTime)
        {
            AttemptPickup();
        }
        else if (Input.GetMouseButtonUp(0) && _isHolding)
        {
            AttemptRelease();
        }
        else if (Input.GetMouseButtonUp(1) && _isHolding)
        {

            AttemptThrow();
        }

        if (_rigidbody != null && _isHolding)
        {
            _previousPosition = _rigidbody.transform.position;
            // Debug.Log(_previousPosition);
        }

    }

    void DetectInteractableCube()
    {
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, detectionRange))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                _rigidbody = hit.collider.GetComponent<Rigidbody>();
            }
            else
            {
                _rigidbody = null;
            }
        }
        else
        {
            _rigidbody = null;
        }


    }

    void AttemptPickup()
    {
        DetectInteractableCube();
        if (_rigidbody == null || _isHolding || Time.time < _nextPickupTime)
        {
            return;
        }
        else
        {
            if (Time.time >= _nextPickupTime)
            {
                _nextPickupTime = Time.time + pickupCooldownDuration;
            }
            _rigidbody.isKinematic = true;
            _rigidbody.interpolation = RigidbodyInterpolation.None;
            _rigidbody.transform.parent = _camera.transform;
            _isHolding = true;
        }
    }

    void AttemptRelease()
    {
        if (_rigidbody == null || !_isHolding)
        {
            return;
        }
        Vector3 releaseVelocity = (_rigidbody.transform.position - _previousPosition) / Time.deltaTime;
        Debug.Log("Released - release velocity = " + releaseVelocity);

        _rigidbody.isKinematic = false;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _rigidbody.transform.parent = null;
        _rigidbody.velocity = releaseVelocity;
        _isHolding = false;
    }

    void AttemptThrow()
    {
        if (_rigidbody == null || !_isHolding || !_camera)
        {
            return;
        }

        Debug.Log("Throwing");
        AttemptRelease();
        _rigidbody.AddForce(_camera.transform.forward * throwForce);

    }

    private void OnDrawGizmos()
    {
        if (_camera == null)
        {
            _camera = GetComponentInChildren<Camera>();
        }

        if (_camera != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_camera.transform.position, _camera.transform.forward * detectionRange);
        }
    }
}
