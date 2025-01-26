using UnityEngine;


public class FPSLimiter : MonoBehaviour

{
    public int FPS = 60;
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = FPS;
    }
}
