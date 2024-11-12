using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private string FirstFloorSceneName = "MainScene";  // Ensure the correct scene name

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Start the scene transition coroutine
            StartCoroutine(TransitionToNewScene());
        }
    }

    private IEnumerator TransitionToNewScene()
    {
        if (string.IsNullOrEmpty(FirstFloorSceneName)) 
        {
            Debug.LogError("Scene name is empty or null! Please set it in the Inspector.");
        }
        else
        {
            Debug.Log("Attempting to load scene: " + FirstFloorSceneName);
            yield return SceneManager.LoadSceneAsync(FirstFloorSceneName);
            SceneManager.UnloadSceneAsync("MainScene");
        }
    }
}