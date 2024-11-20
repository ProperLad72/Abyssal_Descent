using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  // Singleton instance
    public bool exitPlaced = false;      // Tracks if the exit has been placed

    private void Awake()
    {
        // Ensure there's only one instance of the GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keep the GameManager across scenes
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate GameManager instances
        }
    }
}