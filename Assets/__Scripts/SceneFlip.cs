using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class SceneFlip : MonoBehaviour
{
    // Variable to store the name of the previous scene
    private static string lastScene = "";

    // Method to save the current scene name and load a new scene
    private void LoadSceneWithHistory(string sceneName)
    {
        lastScene = SceneManager.GetActiveScene().name;  // Store the current scene
        SceneManager.LoadScene(sceneName);               // Load the new scene
    }

    public void OnStartButtonClicked()
    {
        LoadSceneWithHistory("Game_Scene");
    }

    public void OnSettingsButtonClicked()
    {
        LoadSceneWithHistory("Settings_Scene");
    }

    public void OnBackButtonClicked()
    {
        LoadSceneWithHistory("Title_Scene");
    }

    public void OnOverviewClicked()
    {
        LoadSceneWithHistory("Overview_Scene");
    }

    // Method to return to the last scene
    public void OnReturnToLastSceneClicked()
    {
        if (!string.IsNullOrEmpty(lastScene))
        {
            SceneManager.LoadScene(lastScene);  // Load the last scene
        }
        else
        {
            Debug.LogWarning("No previous scene to return to!");
        }
    }

    public void ReturnButtonClicked()
    {
        SceneManager.LoadScene("OtherGames");
    }
}
