using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public void LoadPairsScene()
    {
        SceneManager.LoadScene("Pairs");
    }

    public void LoadMorePairsScene()
    {
        SceneManager.LoadScene("MorePairs");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartPairs");
    }
}
