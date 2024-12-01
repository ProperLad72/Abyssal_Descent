using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure the player enters the trigger zone
        {
            // Set the exit prefab randomly or cycle through prefabs when the player enters
            //GameManager.Instance.SetExitPrefabRandomly(); // Or SetExitPrefabCycling();
        }
    }
}