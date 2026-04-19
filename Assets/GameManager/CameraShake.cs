using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour 
{

    public static CameraShake instance;

    private Vector3 originalPosition;

    void Awake()
    {
        instance = this;
    }

    // Store the starting position of the camera
    void Start()
    {
        originalPosition = transform.localPosition;
    }

    // This function starts the shake effect
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeCoroutine(duration, magnitude));
    }

    // This creates the shaking motion over time
    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(
                originalPosition.x + x,
                originalPosition.y + y,
                originalPosition.z
            );

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
