using UnityEngine;

public class GeneratorController : MonoBehaviour
{
    [Header("Generator Settings")]
    public float requiredHoldTime = 2f; // How many seconds the player must hold 'E'
    public float interactRange = 3f;    // How close they must stand to the wall
    private float currentHoldTime = 0f;

    public bool isPowered = false;

    [Header("Audio Settings")]
    public AudioSource clickSound;
    public AudioSource alarmSound;

    // This creates a slider in Unity! 0.5 means it waits half a second after the click.
    [Range(0f, 2f)]
    public float delayBeforeAlarm = 0.5f;

    private Camera playerCamera;

    void Start()
    {
        // This automatically finds the player's camera when the game starts
        playerCamera = Camera.main;
    }

    void Update()
    {
        // 1. If the generator is already on, stop running the code
        if (isPowered) return;

        // 2. Shoot an invisible line from the center of the player's screen
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        // 3. Did the line hit anything within 3 meters?
        if (Physics.Raycast(ray, out hit, interactRange))
        {
            // 4. Did the line specifically hit THIS switch?
            if (hit.collider.gameObject == gameObject)
            {
                // The player is looking directly at the switch!
                if (Input.GetKey(KeyCode.E))
                {
                    currentHoldTime += Time.deltaTime; // Start the timer

                    // If they held it long enough, turn it on!
                    if (currentHoldTime >= requiredHoldTime)
                    {
                        TurnOnGenerator();
                    }
                }
                else
                {
                    currentHoldTime = 0f; // Reset timer if they let go early
                }

                // Exit the Update loop here so we don't accidentally reset the timer below
                return;
            }
        }

        // 5. If they look away from the switch, immediately reset the timer
        currentHoldTime = 0f;
    }

    void TurnOnGenerator()
    {
        isPowered = true;
        Debug.Log("Generator ON! Waking up the creature...");

        // 1. Play the instant click sound right away
        if (clickSound != null)
        {
            clickSound.Play();
        }

        // 2. Tell the loud alarm to wait for the click to finish, then play
        if (alarmSound != null)
        {
            alarmSound.PlayDelayed(delayBeforeAlarm);
        }

        // 3. Disable this script so they can't interact with it ever again
        this.enabled = false;
    }
}