using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class GlobalPostProcessing : MonoBehaviour
{
    public static GlobalPostProcessing Instance { get; private set; }
    private PostProcessVolume postProcessVolume;
    private float currentContrast;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupPostProcessing();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void SetupPostProcessing()
    {
        postProcessVolume = gameObject.AddComponent<PostProcessVolume>();
        postProcessVolume.isGlobal = true;
        postProcessVolume.priority = 1;
        postProcessVolume.profile = ScriptableObject.CreateInstance<PostProcessProfile>();
        postProcessVolume.profile.AddSettings<ColorGrading>();
        
        currentContrast = PlayerPrefs.GetFloat("GameContrast", 0f);
        ApplyContrast(currentContrast);
    }

    public void ApplyContrast(float contrastValue)
    {
        currentContrast = contrastValue;
        if (postProcessVolume.profile.TryGetSettings(out ColorGrading colorGrading))
        {
            colorGrading.contrast.Override(currentContrast);
        }
        PlayerPrefs.SetFloat("GameContrast", currentContrast);
        PlayerPrefs.Save();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyContrast(currentContrast);
    }

    public float GetCurrentContrast()
    {
        return currentContrast;
    }
}
