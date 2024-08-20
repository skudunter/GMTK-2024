using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField] private float duration = 0.2f; // How long the shake effect lasts
    [SerializeField] private float frequency = 25.0f; // How fast the shake oscillates

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition; // Store the original position of the camera
    }

    public void Shake(float intensity)
    {
        StartCoroutine(ShakeCoroutine(intensity));
    }

    private IEnumerator ShakeCoroutine(float intensity)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float offsetX = Mathf.PerlinNoise(elapsedTime * frequency, 0f) * 2f - 1f;
            float offsetY = Mathf.PerlinNoise(0f, elapsedTime * frequency) * 2f - 1f;

            Vector3 offset = new Vector3(offsetX, offsetY, 0f) * intensity;
            transform.localPosition = originalPosition + offset;

            elapsedTime += Time.deltaTime;

            yield return null; // Wait until the next frame
        }

        transform.localPosition = originalPosition; // Reset to original position
    }
}
