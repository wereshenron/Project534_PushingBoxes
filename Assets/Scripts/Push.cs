using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour
{
    public float pushForce = 10f;
    public float pullForce = 10f;
    public float detectionRange = 2.0f;

    private Rigidbody _rigidbody;
    private int _hitCount = 0;
    [SerializeField] private Camera _camera;

    void Start()
    {
        if (_camera != null)
        {
            _camera = GetComponentInChildren<Camera>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        DetectThrowableCube();

        if (_rigidbody != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _hitCount++;
                Launch(_camera.transform.forward);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Pull(_camera.transform.forward);
            }
        }

    }

    void DetectThrowableCube()
    {
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, detectionRange))
        {
            if (hit.collider.CompareTag("Interactable") || hit.collider.CompareTag("Interactable"))
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

    void Launch(Vector3 direction)
    {
        float randomX = Random.Range(-0.1f, 0.1f);
        float randomY = Random.Range(-0.1f, 0.1f);
        float randomZ = Random.Range(-0.1f, 0.1f);

        Vector3 randomDirection = direction + new Vector3(randomX, randomY, randomZ);

        _rigidbody.AddForce(randomDirection * pushForce);
    }

    void Pull(Vector3 direction)
    {
        _rigidbody.AddForce(-direction * pullForce);
    }
}
