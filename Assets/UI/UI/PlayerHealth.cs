using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public Image healthFill;

    public float maxHealth = 100f;
    public float currentHealth = 100f;

    [Header("Damage Effects")]
    public AudioSource breathingSound;
    public float breathingDuration = 2f;

    [Header("Camera Shake")]
    public Transform cameraTransform;
    public float shakeDuration = 0.25f;
    public float shakeAmount = 0.08f;

    private Vector3 originalCameraPosition;

    void Start()
    {
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (cameraTransform != null)
        {
            originalCameraPosition = cameraTransform.localPosition;
        }

        UpdateHealthUI();
    }

public void TakeDamage(float damage)
{
    currentHealth -= damage;
    currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

    UpdateHealthUI();

    if (currentHealth > 0)
    {
        PlayDamageEffects();
    }

    if (currentHealth <= 0)
    {
        if (breathingSound != null)
        {
            breathingSound.Stop();
            breathingSound.loop = false;
        }

        Invoke(nameof(TriggerLose), 0.3f);
    }
}

    void UpdateHealthUI()
    {
        float value = currentHealth / maxHealth;

        healthFill.fillAmount = value;

        if (value > 0.75f)
            healthFill.color = Color.green;
        else if (value > 0.45f)
            healthFill.color = Color.yellow;
        else
            healthFill.color = Color.red;
    }

    void PlayDamageEffects()
    {
        // Breathing sound
        if (breathingSound != null)
        {
            breathingSound.Stop();
            breathingSound.loop = true;
            breathingSound.Play();

            CancelInvoke(nameof(StopBreathingSound));
            Invoke(nameof(StopBreathingSound), breathingDuration);
        }

        // Camera shake
        if (cameraTransform != null)
        {
            StopCoroutine(nameof(ShakeCamera));
            StartCoroutine(ShakeCamera());
        }
    }

    IEnumerator ShakeCamera()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeAmount;
            float y = Random.Range(-1f, 1f) * shakeAmount;

            cameraTransform.localPosition = originalCameraPosition + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        cameraTransform.localPosition = originalCameraPosition;
    }

    void StopBreathingSound()
    {
        if (breathingSound != null)
        {
            breathingSound.Stop();
            breathingSound.loop = false;
        }
    }

    void TriggerLose()
    {
        GameManager.instance.LoseGame();
    }
}