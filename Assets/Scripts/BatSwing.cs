using System.Collections;
using UnityEngine;


public class BatSwing : MonoBehaviour
{
    public GameObject bat;  // Assign the baseball bat here in the Inspector
    public float swingSpeed = 500f;
    public float returnSpeed = 300f;
    public float swingDuration = 1.0f;
    public float waitTime = 0.3f;
    private bool isSwinging = false;
    public Transform endRotation;
    private Transform initialRotation;

    void Start()
    {
        // Store the initial rotation for resetting the bat
        initialRotation = bat.transform;
    }

    void Update()
    {
        // Handle swinging the bat when the player clicks or presses a key
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            isSwinging = true;
            StartCoroutine(SwingForward(initialRotation, endRotation, swingDuration));
        }
    }

    private IEnumerator SwingForward(Transform startPosition, Transform endPosition, float duration)
    {
        GameObject tempBat = new GameObject("TempBat");
        Transform tempTransform = tempBat.transform;
        // Swing the bat forward
        float elapsedTime = 0f;
        while (elapsedTime < swingDuration)
        {
            elapsedTime += Time.deltaTime;
            bat.transform.rotation = Quaternion.Slerp(bat.transform.rotation, endPosition.rotation, elapsedTime);
            yield return null;
        }

        // Wait briefly after the swing
        yield return new WaitForSeconds(waitTime);

        // Return the bat to its original position
        elapsedTime = 0f;
        while (elapsedTime < swingDuration)
        {
            elapsedTime += Time.deltaTime;
            bat.transform.rotation = Quaternion.Slerp(bat.transform.localRotation, initialRotation.rotation, elapsedTime);
            yield return null;
        }

        isSwinging = false;
    }
}
