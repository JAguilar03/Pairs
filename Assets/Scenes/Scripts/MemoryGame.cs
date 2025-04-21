using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // For scene management
using UnityEngine.UI; // For UI elements
using System.IO; // For reading the settings file

    public class MemoryGame : MonoBehaviour
    {
        public GameObject[] redItemPrefabs; // Array for red item prefabs
        public GameObject[] blueItemPrefabs; // Array for blue item prefabs

        // UI Elements for panels
        public GameObject greenUIPanel;
        public GameObject yellowUIPanel;

        public RectTransform itemPanel;
        public TextMeshProUGUI countdownText;
        public float countdownDuration = 10f;

        public GameObject winPanel;
        public TextMeshProUGUI winTimeText;
        public Button retryButton;
        public Button menuButton;
        public Button returnButton;
        public RawImage homeIcon;
        public RawImage trophyIcon;

        private List<GameObject> greenRings = new List<GameObject>(); 

        private List<GameObject> instantiatedItems = new List<GameObject>();
        private Vector3[] correctPositions;
        private bool canDrag = false;
        private float elapsedTime = 0f;
        private bool isCountingElapsedTime = false;
        private List<GameObject> activeItems = new List<GameObject>();
        private int matchedItemsCount = 0;

        private AudioSource audioSource;
        private AudioSource countdownAudioSource;

        public AudioClip matchSound1;
        public AudioClip matchSound2;
        public AudioClip winSound;
        public AudioClip countdownSound;

        void Start()
        {
            // Initialize AudioSource components
            audioSource = gameObject.AddComponent<AudioSource>();
            countdownAudioSource = gameObject.AddComponent<AudioSource>();
            countdownAudioSource.clip = countdownSound;
            countdownAudioSource.loop = true;

            // Disable win panel and related UI elements
            winPanel.SetActive(false);
            retryButton.gameObject.SetActive(false);
            menuButton.gameObject.SetActive(false);
            returnButton.gameObject.SetActive(false);

            homeIcon.gameObject.SetActive(false);
            trophyIcon.gameObject.SetActive(false);

            // Add button listeners
            retryButton.onClick.AddListener(RestartGame);
            menuButton.onClick.AddListener(GoToHomeScreen);
            returnButton.onClick.AddListener(GoToHomeScreen);

        // Initialize correctPositions for all items (5 total)
        correctPositions = new Vector3[5];

            // Determine which set of prefabs to use and instantiate them
            ConfigureGameFromSettings();

            SetRandomPositions();
            StartCoroutine(StartMemoryGame());
        }

        void ConfigureGameFromSettings()
        {
            string settingsFilePath = @"C:\\Users\\User\\Documents\\CST-461\\Pairs\\Assets\\Scenes\\settings.txt";

            string colorSetting = "none"; // Default to none if the file can't be read
            if (File.Exists(settingsFilePath))
            {
                colorSetting = File.ReadAllText(settingsFilePath).Trim().ToLower();
            }

            if (colorSetting == "red")
            {
                // Use red items and enable the green UI panel
                InstantiateItems(redItemPrefabs);
                greenUIPanel.SetActive(true);
                yellowUIPanel.SetActive(false);
            }
            else if (colorSetting == "blue")
            {
                // Use blue items and enable the yellow UI panel
                InstantiateItems(blueItemPrefabs);
                greenUIPanel.SetActive(false);
                yellowUIPanel.SetActive(true);
            }
            else
            {
                // Use blue items and enable the yellow UI panel
                InstantiateItems(blueItemPrefabs);
                greenUIPanel.SetActive(false);
                yellowUIPanel.SetActive(false);
            }
        }

        void InstantiateItems(GameObject[] itemPrefabs)
        {
            foreach (GameObject prefab in itemPrefabs)
            {
                GameObject item = Instantiate(prefab);
                item.SetActive(true);
                instantiatedItems.Add(item);
            }
        }

        void SetRandomPositions()
        {
            Vector3 screenBottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
            Vector3 screenTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));

            float minX = screenBottomLeft.x;
            float maxX = screenTopRight.x;
            float minY = screenBottomLeft.y + 1.0f; // Adjusted to prevent overlap with UI
            float maxY = screenTopRight.y - 1.0f; // Adjusted to prevent overlap with UI

            float minSpacing = 1.0f;

            for (int i = 0; i < instantiatedItems.Count; i++)
            {
                Vector3 randomPosition;
                bool positionIsValid;

                do
                {
                    positionIsValid = true;
                    float randomX = Random.Range(minX, maxX);
                    float randomY = Random.Range(minY, maxY);
                    randomPosition = new Vector3(randomX, randomY, 0f);

                    foreach (Vector3 existingPosition in correctPositions)
                    {
                        if (Vector3.Distance(randomPosition, existingPosition) < minSpacing)
                        {
                            positionIsValid = false;
                            break;
                        }
                    }
                }
                while (!positionIsValid);

                instantiatedItems[i].transform.position = randomPosition;
                correctPositions[i] = randomPosition;
            }
        }

        IEnumerator StartMemoryGame()
        {
            float timeRemaining = countdownDuration;
            countdownAudioSource.Play();

            while (timeRemaining > 0)
            {
                countdownText.text = "Memorize: " + Mathf.Ceil(timeRemaining).ToString();
                yield return new WaitForSeconds(1f);
                timeRemaining--;
            }

            countdownAudioSource.Stop();
            countdownText.text = "";
            returnButton.gameObject.SetActive(true);
            homeIcon.gameObject.SetActive(true);

            foreach (GameObject item in instantiatedItems)
            {
                item.SetActive(false);
            }

            List<int> selectedIndices = new List<int>();
            while (selectedIndices.Count < 3)
            {
                int randomIndex = Random.Range(0, instantiatedItems.Count);
                if (!selectedIndices.Contains(randomIndex))
                {
                    selectedIndices.Add(randomIndex);
                    activeItems.Add(instantiatedItems[randomIndex]);
                }
            }

            DisplayChosenObjectsOnPanel();
            canDrag = true;
            isCountingElapsedTime = true;
        }

        void DisplayChosenObjectsOnPanel()
        {
            float minX = -2f;
            float maxX = 2f;
            float spacing = (maxX - minX) / (activeItems.Count - 1);
            float yPosition = itemPanel.position.y;

            for (int i = 0; i < activeItems.Count; i++)
            {
                GameObject item = activeItems[i];
                float xPos = minX + (i * spacing);
                item.SetActive(true);
                item.transform.position = new Vector3(xPos, yPosition, 0);
            }
        }

        void Update()
        {
            if (canDrag)
            {
                HandleDragAndDrop();
            }

            if (isCountingElapsedTime)
            {
                elapsedTime += Time.deltaTime;
                int minutes = Mathf.FloorToInt(elapsedTime / 60);
                int seconds = Mathf.FloorToInt(elapsedTime % 60);
                countdownText.text = $"{minutes:00}:{seconds:00}"; // Display elapsed time in 00:00 format
            }
        }

        // Handle dragging and dropping items
        void HandleDragAndDrop()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null)
                {
                    GameObject selectedItem = hit.collider.gameObject;

                    // Check if the collider is enabled to allow dragging
                    if (selectedItem.GetComponent<Collider2D>().enabled)
                    {
                        StartCoroutine(DragItem(selectedItem));
                    }
                }
            }
        }

        IEnumerator DragItem(GameObject item)
        {
            while (Input.GetMouseButton(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;
                item.transform.position = mousePos;
                yield return null;
            }

            CheckCorrectPlacement(item);
        }

        void DrawGreenRing(GameObject item)
        {
            // Use the item's current position after it has been snapped into place
            Vector3 ringPosition = item.transform.position;

            // Create a new GameObject for the ring
            GameObject ring = new GameObject("GreenRing");
            ring.transform.position = ringPosition; // Set the exact world position

            // Add a LineRenderer component to draw the circle
            LineRenderer lineRenderer = ring.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 100; // Number of segments for a smooth circle
            lineRenderer.loop = true; // Make the line loop to form a circle
            lineRenderer.startWidth = 0.05f; // Width of the line
            lineRenderer.endWidth = 0.05f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;

            // Calculate the radius for the circle
            float radius = 0.7f; // Adjust as needed for your desired ring size

            // Draw the circle
            float angleStep = 360f / lineRenderer.positionCount;
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;
                lineRenderer.SetPosition(i, new Vector3(x, y, 0) + ringPosition);
            }

            // Add the ring to the greenRings list for tracking
            greenRings.Add(ring);
        }

        void CheckCorrectPlacement(GameObject item)
        {
            int itemIndex = instantiatedItems.IndexOf(item);

            if (Vector3.Distance(item.transform.position, correctPositions[itemIndex]) < 0.5f)
            {
                // Snap the object to its correct position
                item.transform.position = correctPositions[itemIndex];
                Debug.Log("Matched: " + item.name);

                // Draw the green ring at the item's snapped position
                DrawGreenRing(item);

                // Disable the collider to prevent further dragging
                Collider2D collider = item.GetComponent<Collider2D>();
                if (collider != null)
                {
                    collider.enabled = false;
                }

                // Play the match sounds
                if (audioSource != null)
                {
                    audioSource.PlayOneShot(matchSound1); // Play the first sound
                    audioSource.PlayOneShot(matchSound2); // Play the second sound
                }

                // Increment the matched items count
                matchedItemsCount++;

                // Check if all items have been matched
                if (matchedItemsCount == activeItems.Count)
                {
                    GameWon();
                }
            }
            else
            {
                Debug.Log("Incorrect placement: " + item.name);
            }
        }


        // Method called when the game is won
        void GameWon()
        {
            // Stop the timer
            isCountingElapsedTime = false;

            // Make all draggable objects and green rings invisible
            foreach (GameObject item in activeItems)
            {
                item.SetActive(false); // Make the draggable object invisible
            }

            foreach (GameObject ring in greenRings)
            {
                ring.SetActive(false); // Make the green ring invisible
            }

            // Play the win sound
            if (audioSource != null && winSound != null)
            {
                audioSource.PlayOneShot(winSound); // Play the win sound once
            }

            // Show the win panel and related UI elements
            winPanel.SetActive(true);
            winTimeText.text = $"Time: {Mathf.FloorToInt(elapsedTime / 60):00}:{Mathf.FloorToInt(elapsedTime % 60):00}";
            retryButton.gameObject.SetActive(true);
            menuButton.gameObject.SetActive(true);
            trophyIcon.gameObject.SetActive(true);
        }

        // Method to restart the game
        void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
        }

        // Method to go to the Home Screen
        void GoToHomeScreen()
        {
            SceneManager.LoadScene("HomeScreen"); // Load the Home Screen scene
        }
    }
