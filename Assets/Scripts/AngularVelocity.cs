using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngularVelocity : MonoBehaviour
{
    public Vector3Int angularVelocity = new(1, 1, 1);
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.angularVelocity = angularVelocity;
    }
}