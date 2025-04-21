using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject cubePrefab;       // Prefab of the cube to instantiate
    public GameSettings gameSettings;   // Link to GameSettings ScriptableObject
    [Range(0f, 0.5f)] public float gap = 0.05f; // Gap between cubes
    [Range(-5f, 5f)] public float xOffset;      // X offset in the editor
    [Range(-5f, 5f)] public float yOffset;      // Y offset in the editor

    private GameObject[,] gridCubes;    // Store the instantiated cubes
    private Vector3 startPosition;      // Starting position of the grid
    private GameObject parentObject;    // Empty parent object to organize the cubes
    public int gridSize;               // Size of the grid

    void Start()
    {
        // Create an empty GameObject to act as the parent for the grid
        parentObject = new GameObject("GridParent");

        // Set gridSize from GameSettings if available
        gridSize = gameSettings != null ? gameSettings.gridSize : 5;
        GenerateGrid();
    }

    // Method to instantiate the grid of cubes
    void GenerateGrid()
    {
        // Clear any existing grid if gridSize has changed
        foreach (Transform child in parentObject.transform)
        {
            Destroy(child.gameObject);
        }

        // Resize the grid array based on the current grid size
        gridCubes = new GameObject[gridSize, gridSize];

        // Calculate cube size (assuming uniform scale)
        Vector3 cubeSize = cubePrefab.transform.localScale;
        float totalGridSize = gridSize * (cubeSize.x + gap); // Total size of the grid including gaps

        // Calculate the start position so the grid is centered
        startPosition = new Vector3(-totalGridSize / 2 + (cubeSize.x / 2), -totalGridSize / 2 + (cubeSize.y / 2), 0);

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 position = startPosition + new Vector3(x * (cubeSize.x + gap), y * (cubeSize.y + gap), 0);
                GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
                cube.transform.SetParent(parentObject.transform);
                cube.name = $"grid [{x}, {y}]";
                gridCubes[x, y] = cube;
            }
        }

        // Initial update of the grid position
        UpdateGridPosition();
    }

    void Update()
    {
        UpdateGridPosition();
    }

    public void UpdateGridPosition()
    {
        Vector3 offset = new Vector3(xOffset, yOffset, 0);
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (gridCubes[x, y] != null)
                {
                    Vector3 originalPosition = startPosition + new Vector3(x * (gridCubes[x, y].transform.localScale.x + gap), y * (gridCubes[x, y].transform.localScale.y + gap), 0);
                    gridCubes[x, y].transform.position = originalPosition + offset;
                }
            }
        }
    }

    // Method to dynamically change grid size
    public void SetGridSize(int newSize)
    {
        if (newSize != gridSize)
        {
            gridSize = newSize;
            GenerateGrid();
        }
    }
}
