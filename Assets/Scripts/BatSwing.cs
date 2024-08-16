using System.Collections;
using UnityEngine;


public class BatSwing : MonoBehaviour
{
    public Transform bat;  // Assign the baseball bat here in the Inspector
    public float swingSpeed = 500f;
    public float returnSpeed = 300f;
    private bool isSwinging = false;
    private Quaternion initialRotation;

    void Start()
    {
        // Store the initial rotation for resetting the bat
        initialRotation = bat.localRotation;
    }

    void Update()
    {
        // Handle swinging the bat when the player clicks or presses a key
        if (Input.GetMouseButtonDown(0) && !isSwinging)
        {
            isSwinging = true;
            StartCoroutine(SwingBat());
        }
    }

    private IEnumerator SwingBat()
    {
        // Swing the bat forward
        float swingTime = 0f;
        while (swingTime < 1f)
        {
            swingTime += Time.deltaTime * swingSpeed;
            bat.localRotation = Quaternion.Slerp(initialRotation, Quaternion.Euler(0, 0, -90), swingTime);
            yield return null;
        }

        // Wait briefly after the swing
        yield return new WaitForSeconds(0.1f);

        // Return the bat to its original position
        float returnTime = 0f;
        while (returnTime < 1f)
        {
            returnTime += Time.deltaTime * returnSpeed;
            bat.localRotation = Quaternion.Slerp(Quaternion.Euler(0, 0, -90), initialRotation, returnTime);
            yield return null;
        }

        isSwinging = false;
    }
}
