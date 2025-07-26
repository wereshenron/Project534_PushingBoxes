using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRbInterpolate : MonoBehaviour
{
    public RigidbodyInterpolation interpolationMode = RigidbodyInterpolation.Interpolate;

    void Awake()
    {
        Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody>();

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.interpolation = interpolationMode;
        }
    }
}
