using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    private Camera _camera;
    public float detectionRange = 10f;
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void OnGUI()
    {
        int size = 12;
        float posX = _camera.pixelWidth/2 - size/4;
        float posY = _camera.pixelHeight/2 -size/2;
        GUI.Label(new Rect(posX, posY, size, size), "*");
    }
    

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * detectionRange);
    }
}
