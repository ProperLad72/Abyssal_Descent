using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transitions : MonoBehaviour
{
    [SerializeField] private string[] floorScenes = { "TutorialFloor", "MainScene", "Secondfloor", "Thirdfloor", "Fourthfloor","Bossfloor" };

    private void Start()
    {
        StartCoroutine(InitializeCurrentFloor());
    }

    private IEnumerator InitializeCurrentFloor()
    {
        // Wait for one frame to ensure the scene is fully loaded
        yield return null;

        string currentSceneName = SceneManager.GetActiveScene().name.Trim().ToLower();
        Debug.Log($"Normalized Active Scene Name: '{currentSceneName}'");

        // Normalize all floor scenes for comparison
        string[] normalizedFloorScenes = new string[floorScenes.Length];
        for (int i = 0; i < floorScenes.Length; i++)
        {
            normalizedFloorScenes[i] = floorScenes[i].Trim().ToLower();
            Debug.Log($"Normalized Scene in floorScenes: '{normalizedFloorScenes[i]}'");
        }

        // Find the index of the current scene
        int index = System.Array.IndexOf(normalizedFloorScenes, currentSceneName);

        if (index >= 0)
        {
            GameManager.Instance.currentFloor = index;
            Debug.Log($"Scene '{SceneManager.GetActiveScene().name}' matched at index {index}. GameManager floor set to {index}.");
        }
        else
        {
            Debug.LogError($"Scene '{SceneManager.GetActiveScene().name}' not found in floorScenes. Defaulting to first floor.");
            GameManager.Instance.currentFloor = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TransitionToNextFloor());
        }
    }

    private IEnumerator TransitionToNextFloor()
    {
        Debug.Log("Attempting to transition to the next floor.");

        int currentFloorIndex = GameManager.Instance.currentFloor;

        if (currentFloorIndex < floorScenes.Length - 1)
        {
            string nextSceneName = floorScenes[currentFloorIndex + 1];
            Debug.Log($"Transitioning to next floor: {nextSceneName}");

            // Load the next scene
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            // Update the GameManager's floor index
            GameManager.Instance.GoToNextFloor();

            Debug.Log($"Transition complete. Current floor index is now {GameManager.Instance.currentFloor}.");
        }
        else
        {
            Debug.Log("This is the last floor. No further transitions available.");
        }
    }

    public IEnumerator TransitionToPreviousFloor()
    {
        Debug.Log("Attempting to transition to the previous floor.");

        int currentFloorIndex = GameManager.Instance.currentFloor;

        if (currentFloorIndex > 0)
        {
            string previousSceneName = floorScenes[currentFloorIndex - 1];
            Debug.Log($"Transitioning to previous floor: {previousSceneName}");

            // Load the previous scene
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(previousSceneName);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            // Decrement the GameManager's floor index
            GameManager.Instance.currentFloor--;
            Debug.Log($"Transition complete. Current floor index is now {GameManager.Instance.currentFloor}.");
        }
        else
        {
            Debug.Log("You are already on the first floor. No further transitions available.");
        }
    }
}