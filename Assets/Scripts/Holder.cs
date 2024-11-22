using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour
{
    public HeldItemsData heldItemsData;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            GameObject childObject = child.gameObject;  
            if (
                childObject.CompareTag("Throwable")
                || childObject.CompareTag("Holdable")
                )
            {
                heldItemsData.currentlyHeldObjects.Add(child.gameObject);
            } 
        }
    }

    // void Update()
    // {

    // }

    
}
