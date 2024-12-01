using UnityEngine;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached the exit!");

            // Move to the next floor
            GameManager.Instance.GoToNextFloor();

            // Find the DungeonManager and regenerate the dungeon
            DungeonManager dungeonManager = FindObjectOfType<DungeonManager>();
            if (dungeonManager != null)
            {
                dungeonManager.GenerateDungeon();
            }
        }
    }
}