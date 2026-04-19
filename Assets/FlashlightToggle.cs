using UnityEngine;

public class FlashlightToggle : MonoBehaviour
{
    public Light flashlight;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.E))
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}