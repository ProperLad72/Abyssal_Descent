using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    // Prefabs
    public GameObject wallPrefab;       // Wall tile prefab
    public GameObject floorPrefab;      // Floor tile prefab
    public GameObject playerPrefab;     // Player prefab
    public GameObject exitPrefab;       // Exit prefab
    public GameObject enemyPrefab;      // Enemy prefab

    // Grid settings
    public int gridWidth = 50;
    public int gridHeight = 50;
    public int numberOfRooms = 5;
    public int corridorWidth = 3; // Width of corridors
    public int numberOfEnemies = 5; // Number of enemies to spawn

    public string[] floorScenes = {"Tutorial","MainScene","Secondfloor","Thirdfloor","Fourthfloor","Bossfloor"};

    public int currentFloorIndex = 1;
    private bool[,] grid; // Tracks grid occupancy

    private List<Vector2Int> roomCenters = new List<Vector2Int>(); // Store room centers for corridor connections

    private static DungeonManager instance;

    public void LoadNextFloor()
    {
        if (currentFloorIndex < floorScenes.Length - 1)
        {
            currentFloorIndex++;
            string nextScene = floorScenes[currentFloorIndex];
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
        }
        else
        {
            Debug.Log("You've reached the final floor!");
        }
    }

    public void LoadPreviousFloor()
    {
        if (currentFloorIndex > 0)
        {
            currentFloorIndex--;
            string previousScene = floorScenes[currentFloorIndex];
            UnityEngine.SceneManagement.SceneManager.LoadScene(previousScene);
        }
        else
        {
            Debug.Log("You are already on the first floor!");
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Initialize the grid
        grid = new bool[gridWidth, gridHeight];

        // Generate the dungeon
        
        //if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "TutorialFloor")
        //{
        GenerateDungeon(); // Call your dungeon generation code
        //}
    }

    public void GenerateDungeon()
    {

        
        // Step 1: Place rooms
        PlaceRooms();

        // Step 2: Connect rooms with corridors
        ConnectRooms();

        // Step 3: Fill empty spaces with walls
        FillEmptySpaces();

        // Step 4: Spawn the player
        SpawnPlayer();

        // Step 5: Place the exit (optional)
        PlaceExit();

        // Step 6: Spawn enemies
        SpawnEnemies();
    }

    

    void PlaceRooms()
    {
        for (int i = 0; i < numberOfRooms; i++)
        {
            // Randomize position within the grid for the room
            int roomWidth = Random.Range(5, 7);
            int roomHeight = Random.Range(5, 7);
            int roomX = Random.Range(0, gridWidth - roomWidth);
            int roomY = Random.Range(0, gridHeight - roomHeight);

            // Check for overlap with other rooms
            if (IsAreaOccupied(roomX, roomY, roomWidth, roomHeight))
            {
                i--; // Retry placement if overlap occurs
                continue;
            }

            // Mark room as occupied and store room center
            MarkGridAsOccupied(roomX, roomY, roomWidth, roomHeight);
            roomCenters.Add(new Vector2Int(roomX + roomWidth / 2, roomY + roomHeight / 2)); // Store room center (approx)

            // Place the room floor
            for (int x = roomX; x < roomX + roomWidth; x++)
            {
                for (int y = roomY; y < roomY + roomHeight; y++)
                {
                    Instantiate(floorPrefab, new Vector3(x, y, 0), Quaternion.identity);
                }
            }
        }
    }

    void ConnectRooms()
    {
        // Connect room centers in sequence
        for (int i = 0; i < roomCenters.Count - 1; i++)
        {
            CreateCorridor(roomCenters[i], roomCenters[i + 1]);
        }
    }

    void CreateCorridor(Vector2Int start, Vector2Int end)
    {
        int x = start.x;
        int y = start.y;

        // Loop until we reach the destination
        while (x != end.x || y != end.y)
        {
            // Carve out a corridor (leave space for walls around)
            for (int dx = -corridorWidth / 2; dx <= corridorWidth / 2; dx++)
            {
                for (int dy = -corridorWidth / 2; dy <= corridorWidth / 2; dy++)
                {
                    int nx = x + dx;
                    int ny = y + dy;

                    // Ensure it's within bounds and mark as floor
                    if (nx >= 0 && ny >= 0 && nx < gridWidth && ny < gridHeight)
                    {
                        grid[nx, ny] = true; // Mark as floor
                        Instantiate(floorPrefab, new Vector3(nx, ny, 0), Quaternion.identity);
                    }
                }
            }

            // Place walls around the current position
            PlaceWallsAround(x, y);

            // Move towards the destination
            if (x < end.x) x++;
            else if (x > end.x) x--;

            if (y < end.y) y++;
            else if (y > end.y) y--;
        }

        // Ensure the final destination has walls around it
        PlaceWallsAround(end.x, end.y);
    }

    void PlaceWallsAround(int x, int y)
    {
        // Check the 8 surrounding tiles
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                int nx = x + dx;
                int ny = y + dy;

                // Skip out-of-bounds tiles
                if (nx < 0 || ny < 0 || nx >= gridWidth || ny >= gridHeight) continue;

                // Skip if already a floor or wall
                if (grid[nx, ny]) continue;

                // Place a wall if it's empty
                Instantiate(wallPrefab, new Vector3(nx, ny, 0), Quaternion.identity);
                grid[nx, ny] = false; // Mark as wall
            }
        }
    }

    void FillEmptySpaces()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // Place walls if the cell is unoccupied (i.e., not a floor or corridor)
                if (!grid[x, y]) // If the cell is empty
                {
                    Instantiate(wallPrefab, new Vector3(x, y, 0), Quaternion.identity);
                    grid[x, y] = false; // Mark as a wall
                }
            }
        }

        // After filling in empty spaces, add boundary walls
        PlaceBoundaryWalls();
    }

    void PlaceBoundaryWalls()
{
    // Top and bottom boundaries
    for (int x = 0; x < gridWidth; x++)
    {
        // Bottom boundary
        Instantiate(wallPrefab, new Vector3(x, 0, 0), Quaternion.identity);
        grid[x, 0] = false;

        // Top boundary
        Instantiate(wallPrefab, new Vector3(x, gridHeight - 1, 0), Quaternion.identity);
        grid[x, gridHeight - 1] = false;
    }

    // Left and right boundaries
    for (int y = 0; y < gridHeight; y++)
    {
        // Left boundary
        Instantiate(wallPrefab, new Vector3(0, y, 0), Quaternion.identity);
        grid[0, y] = false;

        // Right boundary
        Instantiate(wallPrefab, new Vector3(gridWidth - 1, y, 0), Quaternion.identity);
        grid[gridWidth - 1, y] = false;
    }
}

    void SpawnPlayer()
    {
        // Spawn the player at the center of the first room
        if (roomCenters.Count > 0)
        {
            Vector3 spawnPosition = new Vector3(roomCenters[0].x, roomCenters[0].y, -1);
            GameObject playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            
            // Ensure the camera follows the newly instantiated player
            Cinemachine.CinemachineVirtualCamera vcam = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
            if (vcam != null)
            {
                vcam.Follow = playerInstance.transform;
            }
        }
        else
        {
            Debug.LogError("No rooms found to spawn the player!");
        }
    }

    void PlaceExit()
    {
        if (roomCenters.Count > 1)
        {
            Vector3 exitPosition = new Vector3(roomCenters[roomCenters.Count - 1].x, roomCenters[roomCenters.Count - 1].y, -1);
            Instantiate(exitPrefab, exitPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Not enough rooms to place an exit!");
        }
    }

    void SpawnEnemies()
    {
        // Spawn enemies at random positions in the dungeon
        int spawnedEnemies = 0;
        while (spawnedEnemies < numberOfEnemies)
        {
            // Randomly select a position on the grid
            int x = Random.Range(1, gridWidth - 1); // Avoid boundaries
            int y = Random.Range(1, gridHeight - 1);

            // Check if the position is a floor (not a wall)
            if (grid[x, y])
            {
                Instantiate(enemyPrefab, new Vector3(x, y, -3), Quaternion.identity);
                spawnedEnemies++;
            }
        }
    }

    bool IsAreaOccupied(int startX, int startY, int width, int height)
    {
        for (int x = startX; x < startX + width; x++)
        {
            for (int y = startY; y < startY + height; y++)
            {
                if (x >= gridWidth || y >= gridHeight || grid[x, y])
                    return true; // Area is occupied
            }
        }
        return false;
    }

    void MarkGridAsOccupied(int startX, int startY, int width, int height)
    {
        for (int x = startX; x < startX + width; x++)
        {
            for (int y = startY; y < startY + height; y++)
            {
                grid[x, y] = true;
            }
        }
    }
}