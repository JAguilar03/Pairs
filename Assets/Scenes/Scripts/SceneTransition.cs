using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    // This is the name of the scene you want to load.
    [SerializeField] private string sceneToLoad;

    // Optional: Assign the button from the Inspector.
    public Button transitionButton;

    private void Start()
    {
        // Check if the button is assigned, then add a listener to detect button clicks.
        if (transitionButton != null)
        {
            transitionButton.onClick.AddListener(OnButtonClick);
        }
    }

    // This function will be called when the button is clicked.
    public void OnButtonClick()
    {
        LoadScene();
    }

    // Load the scene using the SceneManager.
    private void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
