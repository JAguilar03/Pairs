using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturntoGames : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadOtherGames()
    {
        //SceneManager.LoadScene("OtherGames");
        Application.Quit();
    }
}
