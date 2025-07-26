using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pickup : MonoBehaviour
{


    [Header("Throw Variables")]
    public float maxThrowCharge = 2f;      
    public float maxThrowMultiplier = 3f;     
    private float _currentThrowCharge = 0f;

    [Header("Pickup Variables")]
    public float detectionRange = 2.0f;
    public float throwForce;
    private Rigidbody _itemHeld;
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

        Debug.Log(_currentThrowCharge);

        // Pick that thang up
        if (Input.GetMouseButtonDown(0) && !_isHolding)
        {
            AttemptPickup();
        }

        // Drop it (gently)
        else if (Input.GetMouseButtonUp(0) && _isHolding)
        {
            AttemptRelease();
        }

        if (Input.GetMouseButtonDown(1) && _isHolding)
        {
            _currentThrowCharge = 0f;
        }

        // 🔥 Accumulate charge while holding
        if (Input.GetMouseButton(1) && _isHolding)
        {
            _currentThrowCharge += Time.deltaTime;
            _currentThrowCharge = Mathf.Clamp(_currentThrowCharge, 0f, maxThrowCharge);
        }

        // TROW
        else if (Input.GetMouseButtonUp(1) && _isHolding)
        {

            AttemptThrow();
        }

        if (_itemHeld != null && _isHolding)
        {
            _previousPosition = _itemHeld.transform.position;
        }



    }

    void DetectInteractableCube()
    {
        int layerToIgnore = 1 << 8; // ignore layer 8
        int inverseMask = ~layerToIgnore;

        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, detectionRange, inverseMask))
        {
            Debug.Log(hit.collider.name);
            if (hit.collider.CompareTag("Interactable"))
            {
                _itemHeld = hit.collider.GetComponent<Rigidbody>();
            }
            else
            {
                _itemHeld = null;
            }
        }
        else
        {
            _itemHeld = null;
        }


    }

    void AttemptPickup()
    {
        DetectInteractableCube();
        if (_itemHeld == null || _isHolding)
        {
            return;
        }
        else
        {
            _itemHeld.isKinematic = true;
            _itemHeld.interpolation = RigidbodyInterpolation.None;
            _itemHeld.transform.parent = _camera.transform;
            _isHolding = true;
        }
    }

    bool AttemptRelease(bool isThrown = false)
    {
        if (_itemHeld == null || !_isHolding)
        {
            return false;
        }
        Vector3 releaseVelocity = (_itemHeld.transform.position - _previousPosition) / Time.deltaTime;
        // Debug.Log("Released - release velocity = " + releaseVelocity);

        _itemHeld.isKinematic = false;
        _itemHeld.interpolation = RigidbodyInterpolation.Interpolate;
        _itemHeld.transform.parent = null;

        if (!isThrown)
        {
            _itemHeld.velocity = releaseVelocity;
        }
        _isHolding = false;
        return true;
    }

    void AttemptThrow()
    {
        if (_itemHeld == null || !_isHolding || !_camera)
        {
            return;
        }

        if (AttemptRelease(isThrown: true))
        {
            float chargeRatio = _currentThrowCharge / maxThrowCharge;
            float finalThrowForce = throwForce * Mathf.Lerp(1f, maxThrowMultiplier, chargeRatio);

        
            _itemHeld.AddForce(_camera.transform.forward * finalThrowForce);
            _currentThrowCharge = 0f;
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
