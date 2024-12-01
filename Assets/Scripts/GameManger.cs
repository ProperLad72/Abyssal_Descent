using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int currentFloor = 0; // Start at floor 1
    public bool exitPlaced = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GoToNextFloor()
    {
        currentFloor++;
        exitPlaced = false; // Reset the exit placement for the new floor
    }
}