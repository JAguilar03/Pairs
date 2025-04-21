using UnityEngine;

public class TableCreator : MonoBehaviour
{
    public int rows = 5;
    public int columns = 5;
    public float cellSize = 1f;
    public Color lineColor = Color.black;

    void Start()
    {
        // Create a parent object for the table
        GameObject table = new GameObject("Table");
        table.transform.parent = transform;

        // Calculate the table's dimensions
        float tableWidth = columns * cellSize;
        float tableHeight = rows * cellSize;

        // Get the camera's orthographic size
        Camera mainCamera = Camera.main;
        float cameraHeight = mainCamera.orthographicSize * 2;

        // Calculate the scale factor to fit the table within the camera's view
        float scaleFactor = Mathf.Min(cameraHeight / tableHeight, cameraHeight / tableWidth);

        // Position the table at the center of the camera's view and scale it appropriately
        Vector3 center = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, mainCamera.nearClipPlane));
        table.transform.position = center;
        table.transform.localScale = Vector3.one * scaleFactor;

        // ... (rest of the code remains the same)
    }
}