using UnityEngine;
using UnityEngine.UI;

public class SignInteraction : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject messagePanel; // Reference to the UI panel that shows the message
    public Text messageText;        // Reference to the UI text component for the message

    [Header("Sign Settings")]
    [TextArea]
    public string signMessage = "This is the sign's message."; // Default message text

    private bool playerInRange = false;

    void Start()
    {
        // Ensure the message panel is hidden at the start
        if (messagePanel != null)
            messagePanel.SetActive(false);
    }

    void Update()
    {
        // Check for interaction input and if the player is in range
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleMessage();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            messagePanel.SetActive(false); // Hide message when player leaves range
        }
    }

    private void ToggleMessage()
    {
        if (messagePanel != null && messageText != null)
        {
            // Toggle the panel visibility and set the message text
            messagePanel.SetActive(!messagePanel.activeSelf);
            messageText.text = signMessage;
        }
        else
        {
            Debug.LogWarning("Message panel or message text not assigned.");
        }
    }
}
