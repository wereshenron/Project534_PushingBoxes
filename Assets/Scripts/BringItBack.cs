using UnityEngine;

public class BringItBack : MonoBehaviour
{
    public Vector3 interactableSpawnPoint;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable") || other.CompareTag("Player"))
        {
            other.attachedRigidbody.velocity = Vector3.zero;
            other.attachedRigidbody.angularVelocity = Vector3.zero;
            other.transform.position = interactableSpawnPoint;
        }
    }
}
