using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsPopup : MonoBehaviour
{
    public GameObject controlsPanel;  // Reference to the ControlsPanel

    private void Start()
    {
        // Hide the panel initially
        controlsPanel.SetActive(false);
    }

    private void Update()
    {
        // Toggle panel visibility when "I" is pressed
        if (Input.GetKeyDown(KeyCode.I))
        {
            controlsPanel.SetActive(!controlsPanel.activeSelf);
        }
    }
}