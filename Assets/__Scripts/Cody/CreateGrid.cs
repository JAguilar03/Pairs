using UnityEngine;

public class CreateGrid : MonoBehaviour
{
    public Vector2 gridSize = new Vector2(10f, 10f);
    public Color gridColor = Color.black;
    public int gridDivisions = 5;
    public float lineWidth = 0.05f;

    void Start()
    {
        CreateGridLines();
        CenterInCamera();
    }

    void CreateGridLines()
    {
        GameObject gridParent = new GameObject("Grid");

        float cellWidth = gridSize.x / gridDivisions;
        float cellHeight = gridSize.y / gridDivisions;

        for (int i = 0; i <= gridDivisions; i++)
        {
            // Create horizontal lines
            CreateLine(new Vector2(-gridSize.x / 2, -gridSize.y / 2 + i * cellHeight), 
                       new Vector2(gridSize.x / 2, -gridSize.y / 2 + i * cellHeight), 
                       gridParent.transform);

            // Create vertical lines
            CreateLine(new Vector2(-gridSize.x / 2 + i * cellWidth, -gridSize.y / 2), 
                       new Vector2(-gridSize.x / 2 + i * cellWidth, gridSize.y / 2), 
                       gridParent.transform);
        }
    }

    void CreateLine(Vector2 start, Vector2 end, Transform parent)
    {
        GameObject line = new GameObject("GridLine");
        line.transform.SetParent(parent);

        LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.startColor = gridColor;
        lineRenderer.endColor = gridColor;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = false;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    }

    void CenterInCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(0, 0, -10);
            mainCamera.orthographicSize = gridSize.y / 2;
        }
    }
}
