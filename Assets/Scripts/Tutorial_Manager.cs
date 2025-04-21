using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Manager : MonoBehaviour
{
    public GameObject tutorial_panel; 
    public Button help_button;         
    public Button next_button;        
    public Image tutorial_image;       
    public Sprite[] tutorial_screens;  

    private int curr_screen = 0;
    // Start is called before the first frame update
    void Start()
    {
        // Initially hide the tutorial panel
        tutorial_panel.SetActive(false);

        // Set up button listeners
        help_button.onClick.AddListener(On_Help_Button_Clicked);
        next_button.onClick.AddListener(On_Next_Button_Clicked);
    }

    
    void On_Help_Button_Clicked()
    {
        tutorial_panel.SetActive(true);  // Show the tutorial panel
        Show_Tutorial_Screen();          // Display the first screen
    }

    void On_Next_Button_Clicked()
    {
        curr_screen++;

        // If we've reached the last screen, hide the tutorial panel
        if (curr_screen >= tutorial_screens.Length)
        {
            tutorial_panel.SetActive(false);
            curr_screen = 0; // Optionally reset for next time
        }
        else
        {
            Show_Tutorial_Screen();  // Show the next screen
        }
    }

    void Show_Tutorial_Screen()
    {
        tutorial_image.sprite = tutorial_screens[curr_screen];  // Update the image
    }
}
