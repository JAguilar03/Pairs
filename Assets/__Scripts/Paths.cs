using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Paths : MonoBehaviour
{
    public Material hoverMaterial;
    public Material clickedMaterial;
    public Material originalMaterial;
    public Material pathMaterial;
    public Material wrongPathMaterial;
    public Material missedCubeMaterial;

    private Renderer cubeRenderer;
    private Generator generator;
    private bool isClicked = false;

    private static List<GameObject> clickedCubes = new List<GameObject>();
    public int gridSize;

    private static bool pathComplete = false;
    private static bool userInteractionLocked = true;

    private static List<GameObject> generatedPath = new List<GameObject>();

    void Start()
    {
        cubeRenderer = GetComponent<Renderer>();
        cubeRenderer.material = originalMaterial;

        // Set the path material color to the soft orange
        pathMaterial.color = HexToColor("#f54901");

        // Find the Generator instance
        generator = FindObjectOfType<Generator>();
        if (generator == null)
        {
            Debug.LogError("Generator instance not found.");
            return;
        }

        // Initialize gridSize from generator
        gridSize = generator.gridSize;

        if (gameObject.name == "grid [0, 0]")
        {
            StartCoroutine(GenerateAutomaticPath());
        }
    }

    void OnMouseEnter()
    {
        if (!isClicked && !pathComplete && !userInteractionLocked)
        {
            cubeRenderer.material = hoverMaterial;
        }
    }

    void OnMouseExit()
    {
        if (!isClicked && !pathComplete && !userInteractionLocked)
        {
            cubeRenderer.material = originalMaterial;
        }
    }

    void OnMouseDown()
    {
        if (!isClicked && !pathComplete && !userInteractionLocked)
        {
            isClicked = true;
            cubeRenderer.material = clickedMaterial;
            clickedCubes.Add(gameObject);

            if (clickedCubes[clickedCubes.Count - 1] == generatedPath[clickedCubes.Count - 1]) // Check if correct cube
            {
                ScoreManager.Instance.AddScore(1);
            }

            if (CheckPathCompletion())
            {
                StartCoroutine(ShowCorrectPath());
            }
            else if (IsPathIncorrect())
            {
                StartCoroutine(ShowIncorrectPath());
            }
        }
    }


    IEnumerator GenerateAutomaticPath()
    {
        yield return new WaitForSeconds(2);

        int startY = Random.Range(0, gridSize);
        int endY = Random.Range(0, gridSize);

        GameObject startCube = GameObject.Find($"grid [0, {startY}]");
        GameObject endCube = GameObject.Find($"grid [{gridSize - 1}, {endY}]");

        generatedPath.Clear();
        generatedPath.Add(startCube);

        int currentX = 0;
        int currentY = startY;

        while (currentX < gridSize - 1)
        {
            int nextX = currentX + 1;
            int targetY = (currentX == gridSize - 2) ? endY : currentY + Random.Range(-1, 2);
            targetY = Mathf.Clamp(targetY, 0, gridSize - 1);

            // Fill in intermediate steps
            while (currentY != targetY)
            {
                int step = (targetY > currentY) ? 1 : -1;
                currentY += step;
                GameObject intermediateCube = GameObject.Find($"grid [{currentX}, {currentY}]");
                generatedPath.Add(intermediateCube);
            }

            // Move to the next column
            GameObject nextCube = GameObject.Find($"grid [{nextX}, {targetY}]");
            generatedPath.Add(nextCube);

            currentX = nextX;
            currentY = targetY;
        }

        foreach (GameObject cube in generatedPath)
        {
            Paths pathScript = cube.GetComponent<Paths>();
            pathScript.cubeRenderer.material = pathMaterial;
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(2);

        ResetBoard();
        userInteractionLocked = false;
    }

    bool CheckPathCompletion()
    {
        return clickedCubes.Count == generatedPath.Count && !IsPathIncorrect();
    }

    bool IsPathIncorrect()
    {
        for (int i = 0; i < clickedCubes.Count; i++)
        {
            if (i >= generatedPath.Count || clickedCubes[i] != generatedPath[i])
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator ShowIncorrectPath()
    {
        userInteractionLocked = true;

        // Find the index where the path went wrong
        int wrongIndex = clickedCubes.Count;
        for (int i = 0; i < clickedCubes.Count; i++)
        {
            if (i >= generatedPath.Count || clickedCubes[i] != generatedPath[i])
            {
                wrongIndex = i;
                break;
            }
        }

        // Show missed cubes, including the one where the player went wrong
        for (int i = wrongIndex; i < generatedPath.Count; i++)
        {
            Paths pathScript = generatedPath[i].GetComponent<Paths>();
            pathScript.cubeRenderer.material = missedCubeMaterial;
        }

        // Pulse effect for wrong cubes
        for (int i = 0; i < 3; i++) // Pulse 3 times
        {
            for (int j = wrongIndex; j < clickedCubes.Count; j++)
            {
                Paths pathScript = clickedCubes[j].GetComponent<Paths>();
                pathScript.cubeRenderer.material = wrongPathMaterial;
            }
            yield return new WaitForSeconds(0.3f);

            for (int j = wrongIndex; j < clickedCubes.Count; j++)
            {
                Paths pathScript = clickedCubes[j].GetComponent<Paths>();
                pathScript.cubeRenderer.material = clickedMaterial;
            }
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(1f);

        ResetBoard();
        StartCoroutine(GenerateAutomaticPath());
    }

    IEnumerator ShowCorrectPath()
    {
        pathComplete = true;
        userInteractionLocked = true;

        // Flash effect for correct path
        for (int i = 0; i < 3; i++) // Flash 3 times
        {
            foreach (GameObject cube in clickedCubes)
            {
                Paths pathScript = cube.GetComponent<Paths>();
                pathScript.cubeRenderer.material = hoverMaterial;
            }
            yield return new WaitForSeconds(0.3f);

            foreach (GameObject cube in clickedCubes)
            {
                Paths pathScript = cube.GetComponent<Paths>();
                pathScript.cubeRenderer.material = clickedMaterial;
            }
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(1f);

        ResetBoard();
        StartCoroutine(GenerateAutomaticPath());
    }

    void ResetBoard()
    {
        foreach (GameObject cube in clickedCubes)
        {
            Paths pathScript = cube.GetComponent<Paths>();
            pathScript.cubeRenderer.material = originalMaterial;
            pathScript.isClicked = false;
        }

        foreach (GameObject cube in generatedPath)
        {
            Paths pathScript = cube.GetComponent<Paths>();
            pathScript.cubeRenderer.material = originalMaterial;
        }

        clickedCubes.Clear();
        pathComplete = false;
    }

    Color HexToColor(string hex)
    {
        Color color = Color.black;
        ColorUtility.TryParseHtmlString(hex, out color);
        return color;
    }
}
