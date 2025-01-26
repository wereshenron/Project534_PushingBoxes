using UnityEngine;

public class Pickup : MonoBehaviour
{

    public float detectionRange = 2.0f;
    public bool isHolding = false;
    public int layerMask;
    private Rigidbody _rigidbody;
    private Vector3 _previousPosition;
    [SerializeField] private Camera _camera;

    void Start()
    {
        if (_camera == null)
        {
            _camera = transform.GetComponent<Camera>();
        }

        layerMask = ~LayerMask.GetMask("Player");
    }

    void Update()
    {

        if (_rigidbody != null && isHolding)
        {
            _previousPosition = _rigidbody.transform.position;
        }

    }

    void DetectInteractableCube()
    {

        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, detectionRange, layerMask))
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

    public void AttemptPickup(Rigidbody rigidbody)
    {
        // DetectInteractableCube();
        if (rigidbody == null || isHolding)
        {
            return;
        }

        rigidbody.isKinematic = true;
        rigidbody.transform.parent = _camera.transform;
        _rigidbody = rigidbody;
        isHolding = true;
    }

    public void AttemptRelease()
    {
        if (_rigidbody == null || !isHolding)
        {
            return;
        }
        Vector3 releaseVelocity = (_rigidbody.transform.position - _previousPosition) / Time.deltaTime;

        _rigidbody.isKinematic = false;
        _rigidbody.transform.parent = null;
        _rigidbody.velocity = releaseVelocity;
        isHolding = false;
        _rigidbody = null;
    }

    void AttemptThrow()
    {
        if (_rigidbody == null || !isHolding)
        {
            return;
        }

    }

    // private void OnDrawGizmos()
    // {
    //     if (_camera == null)
    //     {
    //         _camera = GetComponentInChildren<Camera>();
    //     }

    //     if (_camera != null)
    //     {
    //         Gizmos.color = Color.red;
    //         Gizmos.DrawRay(_camera.transform.position, _camera.transform.forward * detectionRange);
    //     }
    // }
}
