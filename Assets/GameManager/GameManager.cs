using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
    public static GameManager instance;
    [Header("Generator Settings")]
    public int totalRealGenerators = 4;     // How many real generators needed to win
    public int activatedRealGenerators = 0;   // How many are activated

    [Header("Game Objects")]
    public GameObject exitDoor;

    [Header("UI")]
    public Text messageText;

    [Header("Audio")]
    public AudioSource fakeGeneratorSound;
    public AudioSource generatorStartUpSound;
    public AudioSource monsterSound;



    [Header("Monster Settings")]
    public bool monsterAwake = false;
    public float monsterSpeed = 3f;

    private CameraShake cameraShake;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        cameraShake = FindAnyObjectByType<CameraShake>();
    }

    // Called whenever a generator is activated
    public void OnGeneratorTriggered(bool isRealGenerator)
    {
        Debug.Log("Generator Triggered!");

        // If it's a fake generator
        if (!isRealGenerator)
        {
            fakeGeneratorSound.Play();
            messageText.text = "Something is wrong...";

            monsterSound.Play();
            // Make monster slightly faster
            monsterSpeed = Mathf.Min(monsterSpeed + 1f, 20f);

            // screen shake effect
            if (cameraShake != null)
                cameraShake.Shake(0.3f, 0.2f);

            return;
        }

        // If it's a real generator
        generatorStartUpSound.Play();
        activatedRealGenerators++;
        messageText.text = "Generator activated: " + activatedRealGenerators;

        // Wake monster on first activation
        if (!monsterAwake && activatedRealGenerators >=2)
        {
            monsterAwake = true;
        }

        // Increase difficulty over time
        IncreaseDifficulty();

        // Check win condition
        if (activatedRealGenerators >= totalRealGenerators)
        {
            OpenExit();
            WinGame();
        }
    }

    // Increase monster difficulty slightly
    void IncreaseDifficulty()
    {
        monsterSpeed = Mathf.Min(monsterSpeed + 0.5f, 20f);
    }

    // Open exit door when player wins
    void OpenExit()
    {
        if (exitDoor != null)
            exitDoor.SetActive(true);
    }

    void WinGame()
    {
        messageText.text = "YOU WIN!";
        monsterAwake = false;       // stop monster
        monsterSpeed = 0f;
    }

    public void LoseGame()
    {
        messageText.text = "YOU LOSE!";
        monsterAwake = false;
        monsterSpeed = 0f;
    }
    }
