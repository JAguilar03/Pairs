using UnityEngine;
using UnityEngine.SceneManagement; 

public class StartMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("DifficultySelect");
    }

    public void SettingsGame() 
    {
        SceneManager.LoadScene("GameSettings");
    }
}
