using UnityEngine;

public class ExtraFog : MonoBehaviour
{
    private void OnEnable()
    {
        RenderSettings.fogDensity = 0.011f;
    }

    private void OnDisable()
    {
        RenderSettings.fogDensity = 0.007f;
    }
}
