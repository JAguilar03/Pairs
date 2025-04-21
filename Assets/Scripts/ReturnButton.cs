using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnButton : MonoBehaviour
{
    public void ReturntoMainMenu()
    {
        SceneManager.LoadScene("DifficultySelect");
    }
}
