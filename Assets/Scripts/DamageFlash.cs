using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageFlash : MonoBehaviour
{
    private Image damageImage; // Reference to the Image component
    public float flashDuration = 0.2f; // How long the screen should flash red
    public Color flashColor = Color.red; // Color of the flash (red for damage)

    private void Awake()
    {
        damageImage = GetComponent<Image>();
        if (damageImage == null)
        {
            Debug.LogError("DamageFlash script requires an Image component.");
        }
        damageImage.enabled = false; // Start with the flash image hidden
    }

    public void FlashScreen()
    {
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        damageImage.color = flashColor; // Set the flash color
        damageImage.enabled = true; // Show the flash image

        float elapsedTime = 0f;
        while (elapsedTime < flashDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        damageImage.enabled = false; // Hide the flash image after the duration
    }
}
