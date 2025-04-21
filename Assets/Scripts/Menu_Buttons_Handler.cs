using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Buttons_Handler : MonoBehaviour
{

    public Button start_button;
    public Button help_button;

    // Start is called before the first frame update
    void Start()
    {
        start_button.onClick.AddListener(LoadSceneOnClick);
    }

    void LoadSceneOnClick()
    {
        // Replace "YourSceneName" with the name of the scene you want to load
        SceneManager.LoadScene("MemoryManGame");
    }


    // Add this method to load the scene
    public void ReturnToMainScene()
    {
        // Replace "MainScene" with the name of the scene you want to load
        SceneManager.LoadScene("Game_Scene");  
    }

    public void ReturnToOtherGames()
    {
        SceneManager.LoadScene("OtherGames");
    }



}
