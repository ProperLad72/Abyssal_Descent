using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    // Name of the scene to load
    [SerializeField] private string targetSceneName;

    // Trigger event to check when player enters the collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object entering the trigger is tagged "Player"
        if (other.CompareTag("Player"))
        {
            // Load the specified target scene
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
