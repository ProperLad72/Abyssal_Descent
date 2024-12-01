using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    // Prefabs
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject playerPrefab;
    public GameObject exitPrefab;
    public GameObject enemyPrefab;

    // Grid settings
    public int gridWidth = 50;
    public int gridHeight = 50;
    public int numberOfRooms = 5;
    public int corridorWidth = 3;
    public int numberOfEnemies = 5;

    public string[] floorScenes = { "Tutorial", "MainScene", "Secondfloor", "Thirdfloor", "Fourthfloor", "Bossfloor" };

    public int currentFloorIndex = 1;
    private bool[,] grid;
    private List<Vector2Int> roomCenters = new List<Vector2Int>();
    private static DungeonManager instance;

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
        grid = new bool[gridWidth, gridHeight];
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        PlaceRooms();
        ConnectRooms();
        FillEmptySpaces();
        PlaceBoundaryWalls();
        SpawnPlayer();
        PlaceExit();
        SpawnEnemies();
    }

    void PlaceRooms()
    {
        for (int i = 0; i < numberOfRooms; i++)
        {
            int roomWidth = Random.Range(5, 7);
            int roomHeight = Random.Range(5, 7);
            int roomX = Random.Range(0, gridWidth - roomWidth);
            int roomY = Random.Range(0, gridHeight - roomHeight);

            if (IsAreaOccupied(roomX, roomY, roomWidth, roomHeight))
            {
                i--;
                continue;
            }

            MarkGridAsOccupied(roomX, roomY, roomWidth, roomHeight);
            roomCenters.Add(new Vector2Int(roomX + roomWidth / 2, roomY + roomHeight / 2));

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
        for (int i = 0; i < roomCenters.Count - 1; i++)
        {
            CreateCorridor(roomCenters[i], roomCenters[i + 1]);
        }
    }

    void CreateCorridor(Vector2Int start, Vector2Int end)
    {
        int x = start.x;
        int y = start.y;

        while (x != end.x || y != end.y)
        {
            for (int dx = -corridorWidth / 2; dx <= corridorWidth / 2; dx++)
            {
                for (int dy = -corridorWidth / 2; dy <= corridorWidth / 2; dy++)
                {
                    int nx = x + dx;
                    int ny = y + dy;
                    if (nx >= 0 && ny >= 0 && nx < gridWidth && ny < gridHeight)
                    {
                        grid[nx, ny] = true;
                        Instantiate(floorPrefab, new Vector3(nx, ny, 0), Quaternion.identity);
                    }
                }
            }
            if (x < end.x) x++;
            else if (x > end.x) x--;
            if (y < end.y) y++;
            else if (y > end.y) y--;
        }
    }

    void FillEmptySpaces()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (!grid[x, y])
                {
                    Instantiate(wallPrefab, new Vector3(x, y, 0), Quaternion.identity);
                }
            }
        }
    }

    void PlaceBoundaryWalls()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            Instantiate(wallPrefab, new Vector3(x, 0, 0), Quaternion.identity);
            Instantiate(wallPrefab, new Vector3(x, gridHeight - 1, 0), Quaternion.identity);
        }
        for (int y = 0; y < gridHeight; y++)
        {
            Instantiate(wallPrefab, new Vector3(0, y, 0), Quaternion.identity);
            Instantiate(wallPrefab, new Vector3(gridWidth - 1, y, 0), Quaternion.identity);
        }
    }

    void SpawnPlayer()
    {
        if (roomCenters.Count > 0)
        {
            Vector3 spawnPosition = new Vector3(roomCenters[0].x, roomCenters[0].y, -1);
            GameObject playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);

            var vcam = FindObjectOfType<Cinemachine.CinemachineVirtualCamera>();
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
            Vector3 exitPosition = new Vector3(roomCenters[^1].x, roomCenters[^1].y, -1);
            Instantiate(exitPrefab, exitPosition, Quaternion.identity);
        }
    }

    void SpawnEnemies()
    {
        int spawnedEnemies = 0;
        while (spawnedEnemies < numberOfEnemies)
        {
            int x = Random.Range(1, gridWidth - 1); // Avoid boundaries
            int y = Random.Range(1, gridHeight - 1);

            if (grid[x, y]) // Only spawn on valid floor tiles
            {
                GameObject enemyInstance = Instantiate(enemyPrefab, new Vector3(x, y, -0.5f), Quaternion.identity);

                // Adjust sprite sorting order
                SpriteRenderer spriteRenderer = enemyInstance.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sortingOrder = 2; // Ensure enemies appear above the floor
                }

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
                    return true;
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
