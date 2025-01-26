using UnityEngine;

public class Push : MonoBehaviour
{
    public float pushForce = 10f;
    public float pullForce = 10f;
    public float detectionRange = 2.0f;
    private Rigidbody _rigidbody;
    [SerializeField] private Camera _camera;
    
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Launch(_camera.transform.forward);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Pull(_camera.transform.forward);
        }

    }

    void DetectInteractableCube()
    {
        int layerMask = ~LayerMask.GetMask("Player");

        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, detectionRange, layerMask))
        {
            _rigidbody = hit.collider.GetComponent<Rigidbody>();
        }
        else
        {
            _rigidbody = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (_camera == null)
        {
            _camera = transform.GetComponent<Camera>();
        }

        if (_camera != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_camera.transform.position, _camera.transform.forward * detectionRange);
        }
    }

    void Launch(Vector3 direction)
    {
        DetectInteractableCube();
        if (_rigidbody == null)
        {
            return;
        }
        float randomX = Random.Range(-0.1f, 0.1f);
        float randomY = Random.Range(-0.1f, 0.1f);
        float randomZ = Random.Range(-0.1f, 0.1f);

        Vector3 randomDirection = direction + new Vector3(randomX, randomY, randomZ);

        _rigidbody.AddForce(randomDirection * pushForce);
        _rigidbody = null;
    }

    void Pull(Vector3 direction)
    {
        DetectInteractableCube();
        if (_rigidbody == null)
        {
            return;
        }

        _rigidbody.AddForce(-direction * pullForce);
        _rigidbody = null;
    }
}
