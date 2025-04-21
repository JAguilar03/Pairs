using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MenuManager : MonoBehaviour
{
    public Button noneButton;
    public Button blueButton;
    public Button redButton;
    private string settingsFilePath = "C:\\Users\\User\\Documents\\CST-461\\Pairs\\Assets\\Scenes\\settings.txt";

    private void Start()
    {
        // Set up button listeners
        noneButton.onClick.AddListener(() => SaveColorSetting("None"));
        blueButton.onClick.AddListener(() => SaveColorSetting("Blue"));
        redButton.onClick.AddListener(() => SaveColorSetting("Red"));
    }

    private void SaveColorSetting(string colorSetting)
    {
        File.WriteAllText(settingsFilePath, colorSetting);
        Debug.Log("Color setting saved: " + colorSetting);
    }
}
