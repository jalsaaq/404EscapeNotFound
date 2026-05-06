using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void Retry()
    {
        SceneManager.LoadScene("escape404");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("escape404");
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("StartPoint");
    }
}