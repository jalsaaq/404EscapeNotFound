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
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool isMoving = x != 0 || z != 0;

     
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && isMoving && currentStamina > 0;

        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

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