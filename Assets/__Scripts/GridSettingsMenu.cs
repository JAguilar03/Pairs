using UnityEngine;
using UnityEngine.UI;

public class GridSettingsMenu : MonoBehaviour
{
    public GameSettings gameSettings;  // Reference to the GameSettings ScriptableObject
    public Slider gridSizeSlider;

    void Start()
    {
        // Initialize the slider's value to the current grid size in GameSettings
        gridSizeSlider.value = gameSettings.gridSize;
        gridSizeSlider.onValueChanged.AddListener(UpdateGridSize);
    }

    void UpdateGridSize(float size)
    {
        gameSettings.gridSize = Mathf.RoundToInt(size);  // Update grid size in GameSettings
    }
}
