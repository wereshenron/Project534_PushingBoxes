using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRbInterpolate : MonoBehaviour
{
    public RigidbodyInterpolation interpolationMode = RigidbodyInterpolation.Interpolate;

    void Start()
    {
        Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody>();

        foreach (Rigidbody rb in rigidbodies)
        {
            rb.interpolation = interpolationMode;
        }

        Debug.Log($"Set interpolation mode to {interpolationMode} for {rigidbodies.Length} rigidbodies.");
    }
}
