using UnityEngine;
using UnityEngine.UI;

public class playerMovment : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;

    public float maxStamina = 5f;
    public float currentStamina;
    public float drainRate = 1f;
    public float regenRate = 0.8f;

    public Slider staminaBar;

    // --- NEW GRAVITY VARIABLES ---
    public float gravity = -9.81f; // Standard Earth gravity
    private Vector3 velocity; // Stores our falling speed
    // -----------------------------

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;

        staminaBar.maxValue = maxStamina;
        staminaBar.value = currentStamina;
    }

    void Update()
    {
        // --- NEW GROUND CHECK ---
        // If we are touching the floor, stop gravity from building up infinitely
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // A small downward force to keep the player snapped to the floor
        }
        // ------------------------

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool isMoving = x != 0 || z != 0;
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && isMoving && currentStamina > 0;

        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // --- NEW GRAVITY MATH ---
        // Apply gravity to our vertical velocity over time, then move the controller down
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        // ------------------------

        // stamina
        if (isRunning)
        {
            currentStamina -= drainRate * Time.deltaTime;
        }
        else
        {
            currentStamina += regenRate * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        staminaBar.value = currentStamina;
    }
}