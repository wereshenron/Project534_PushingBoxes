using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Push : MonoBehaviour
{
    public float pushForce = 10f;
    public float detectionRange = 2.0f;
    
    private Rigidbody rb;
    // private int _hitCount = 0;
    private Camera _camera;
    private Vector3 _pushForward;
    private Vector3 _pullUp;

    void Start()
    {
        _camera = GetComponentInChildren<Camera>();

        if (_camera != null)
        {
            _pushForward = _camera.transform.forward;
            _pullUp = _camera.transform.up;
        }
    }

    // Update is called once per frame
    void Update()
    {
        DetectCube();

        if (rb != null) 
        {
            if (Input.GetMouseButton(0))
            {
                Launch(transform.forward);
                Debug.Log("Hit");
            } 
            else if (Input.GetMouseButton(1))
            {
                Launch(-transform.forward);
            }
        }
        
    }

    void DetectCube() {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, detectionRange))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                rb = hit.collider.GetComponent<Rigidbody>();
            }
            else
            {
                rb = null;
            }
        } 
        else 
        {
            rb = null;
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * detectionRange);
    }

    void Launch(Vector3 direction)
    {
        rb.AddForce(direction * pushForce);
    }
}
