using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transitions : MonoBehaviour
{
    [SerializeField] private string secondFloorSceneName = "Secondfloor";  // Set this to your actual scene name in the Inspector

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
        if (string.IsNullOrEmpty(secondFloorSceneName)) 
        {
            Debug.LogError("Scene name is empty or null! Please set it in the Inspector.");
        }
        else
        {
            Debug.Log("Attempting to load scene: " + secondFloorSceneName);
            yield return SceneManager.LoadSceneAsync(secondFloorSceneName);
            SceneManager.UnloadSceneAsync("MainScene");
        }
    }
}