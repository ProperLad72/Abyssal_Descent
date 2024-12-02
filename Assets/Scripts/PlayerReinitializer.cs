using UnityEngine;

public class PlayerReinitializer : MonoBehaviour
{
    private void Start()
    {
        // Re-enable collision layers
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
}