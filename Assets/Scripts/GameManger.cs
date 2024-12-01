using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Floor management
    public int currentFloor = 0; // Start at floor 1
    public bool exitPlaced = false;

    // Player health management
    public float playerHealth = 100f; // Persisted player health
    public float maxHealth = 100f;    // Default max health for the player

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

    // Method to transition to the next floor
    public void GoToNextFloor()
    {
        currentFloor++;
        exitPlaced = false; // Reset the exit placement for the new floor
    }

    // Method to update the player's health (called by the PlayerHealth script)
    public void UpdatePlayerHealth(float newHealth)
    {
        playerHealth = Mathf.Clamp(newHealth, 0f, maxHealth); // Ensure health stays in valid range
    }

    // Method to reset player health to maximum (e.g., on respawn)
    public void ResetPlayerHealth()
    {
        playerHealth = maxHealth;
    }

     public void ResetGame()
    {
        Debug.Log("Resetting game state...");
        currentFloor = 0; // Reset to the initial floor
        exitPlaced = false; // Reset exit placement
        // Add any additional reset logic here (e.g., reset score, inventory)
    }
}