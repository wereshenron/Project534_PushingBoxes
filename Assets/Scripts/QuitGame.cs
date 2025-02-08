using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    public Button quitButton; // Reference to the UI Button

    public void Start()
    {
        quitButton.onClick.AddListener(Quit); // Assign the Quit function
        Debug.Log("Set");
    }

    public void Quit()
    {
        Debug.Log("Quit Game");
        Application.Quit();
        
        // If running in the editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}