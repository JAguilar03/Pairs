using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;
    private AudioSource audioSource;
    private string currentScene;
    
    void Awake() 
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            audioSource = GetComponent<AudioSource>();
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log("Background Music Initialized");
        } 
        else 
        {
            Debug.Log("Destroying duplicate BackgroundMusic");
            Destroy(gameObject);
            return;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene Loaded: {scene.name} - Instance exists: {instance != null}");
        currentScene = scene.name;
        
        if (scene.name == "OtherGames")
        {
            audioSource.Stop();
            Debug.Log("Music Stopped");
        }
        else if (scene.name == "Pairs" || scene.name == "StartPairs")
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                Debug.Log("Music Started Playing");
            }
        }
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        Debug.Log("BackgroundMusic OnDestroy called");
    }
}
