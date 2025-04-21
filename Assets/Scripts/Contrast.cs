using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class Contrast : MonoBehaviour
{
    public PostProcessVolume postProcessVolume;
    public Slider contrastSlider;
    private ColorGrading colorGrading;

    private void Start()
    {
        if (postProcessVolume != null)
        {
            postProcessVolume.profile.TryGetSettings(out colorGrading);
        }
        
        if (contrastSlider != null)
        {
            contrastSlider.onValueChanged.AddListener(AdjustContrast);
        }
    }

    public void AdjustContrast(float value)
    {
        if (colorGrading != null)
        {
            colorGrading.contrast.Override(value);
            
        }
    }
}
