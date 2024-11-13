using UnityEngine;
using UnityEngine.UI;

public class SignInteraction : MonoBehaviour
{
    public GameObject infoPromptPanel; // Assign in inspector
    public Text infoText; // Assign in inspector
    public string message = "This is information about this area."; // Customize message

    private bool isNearSign = false;

    void Update()
    {
        // Show the info prompt when near the sign and pressing 'E'
        if (isNearSign && Input.GetKeyDown(KeyCode.E))
        {
            infoPromptPanel.SetActive(true);
            infoText.text = message;
        }

        // Close the info prompt when 'Escape' is pressed
        if (infoPromptPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            infoPromptPanel.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNearSign = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isNearSign = false;
            infoPromptPanel.SetActive(false); // Ensure prompt closes when leaving
        }
    }
}