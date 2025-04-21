using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSelector : MonoBehaviour
{
    public void LoadPairsGame()
    {
        SceneManager.LoadScene("StartPairs");
    }

    public void LoadMemoryMan() 
    {
        SceneManager.LoadScene("Start_Screen");
    }

    public void LoadMemoryBank() 
    {
        SceneManager.LoadScene("MemoryBank");
    }

    public void LoadFacialMemory() 
    {
        SceneManager.LoadScene("FacialMemory");
    }

    public void LoadHowIRememberIt()
    {
        SceneManager.LoadScene("HomeScreen");
    }

    public void LoadMelodyMatch()
    {
        SceneManager.LoadScene("MelodyMatch");
    }

    public void LoadMemoryMapping()
    {
        SceneManager.LoadScene("Title_Scene");
    }

    public void LoadMemoryGame() 
    {
        SceneManager.LoadScene("MemoryGame");
    }
}
