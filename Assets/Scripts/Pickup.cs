using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    public float detectionRange = 2.0f;
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
        if (Input.GetKeyDown(KeyCode.E) && !_isHolding)
        {
            AttemptPickup();
        }
        else if (Input.GetKeyDown(KeyCode.E) && _isHolding)
        {
            AttemptRelease();
        }
        else if (Input.GetKeyDown(KeyCode.F) && _isHolding)
        {
            AttemptThrow();
        }

        if (_rigidbody != null && _isHolding)
        {
            _previousPosition = _rigidbody.transform.position;
            Debug.Log(_previousPosition);
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
        if (_rigidbody == null || _isHolding)
        {
            return;
        }

        _rigidbody.isKinematic = true;
        _rigidbody.transform.parent = _camera.transform;
        _isHolding = true;
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
        _rigidbody.transform.parent = null;
        _rigidbody.velocity = releaseVelocity;
        _isHolding = false;
    }

    void AttemptThrow()
    {
        if (_rigidbody == null || !_isHolding)
        {
            return;
        }

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
