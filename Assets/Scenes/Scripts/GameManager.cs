using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject markerPrefab;
    public GameObject draggableObjectPrefab;
    public int numberOfMarkers = 3;
    public Vector2 markerArea = new Vector2(5, 5);
    public Vector2 draggableArea = new Vector2(-5, -2);
    public Text timerText; // UI Text element for displaying the timer

    private float countdown = 5.0f; // Countdown duration in seconds
    private bool isCountingDown = true;
    private float elapsedTime = 0f; // Elapsed time after the countdown

    // List to hold colors used for markers and draggable objects
    private List<Color> colors = new List<Color> { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta };
    private List<Color> usedColors = new List<Color>();

    void Start()
    {
        SpawnMarkers();
        SpawnDraggableObjects();
    }

    void Update()
    {
        if (isCountingDown)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0)
            {
                countdown = 0;
                isCountingDown = false;
            }

            // Update the timer text with countdown in seconds
            timerText.text = countdown.ToString("F1"); // Format with one decimal place
        }
        else
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            timerText.text = $"{minutes:00}:{seconds:00}"; // Format as 00:00
        }
    }

    void SpawnMarkers()
    {
        for (int i = 0; i < numberOfMarkers; i++)
        {
            Color randomColor = GetUniqueRandomColor();
            Vector3 randomPosition = new Vector3(Random.Range(-markerArea.x, markerArea.x), Random.Range(-markerArea.y, markerArea.y), 0);
            GameObject marker = Instantiate(markerPrefab, randomPosition, Quaternion.identity);
            marker.tag = "Marker";
            marker.GetComponent<Marker>().markerColor = randomColor;
        }
    }

    void SpawnDraggableObjects()
    {
        for (int i = 0; i < numberOfMarkers; i++)
        {
            Color randomColor = usedColors[i];
            Vector3 randomPosition = new Vector3(Random.Range(draggableArea.x, -draggableArea.x), draggableArea.y, 0);
            GameObject draggableObject = Instantiate(draggableObjectPrefab, randomPosition, Quaternion.identity);
            draggableObject.GetComponent<Draggable>().objectColor = randomColor;
        }
    }

    Color GetUniqueRandomColor()
    {
        if (colors.Count == 0) return Color.white;

        int randomIndex = Random.Range(0, colors.Count);
        Color selectedColor = colors[randomIndex];

        usedColors.Add(selectedColor);
        colors.RemoveAt(randomIndex);

        return selectedColor;
    }
}
