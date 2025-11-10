using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraEffects : MonoBehaviour
{
    public static CameraEffects Instance;

    [Header("Shake Settings")]
    public float shakeDuration = 0.3f;
    public float shakeMagnitude = 0.2f;


    [Header("Flash Settings")]
    public RawImage flashImage; // UI Image covering the screen
    public Color flashColor = new Color(1, 0, 0, 0.4f);
    public float flashFadeSpeed = 3f;
    public AudioSource crackSFX;

    private Vector3 originalPos;
    private Coroutine shakeCoroutine;
    private Color originalColor = Color.clear;

    void Awake()
    {
        Instance = this;
        originalPos = transform.localPosition;
        if (flashImage != null)
            flashImage.color = originalColor;
    }

    public void PlayDamageEffect()
    {
        if (shakeCoroutine != null) StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(Shake());
        StartCoroutine(FlashRed());
    }

    IEnumerator Shake()
    {
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            Vector3 randomPoint = originalPos + Random.insideUnitSphere * shakeMagnitude;
            transform.localPosition = new Vector3(randomPoint.x, randomPoint.y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
    }

    IEnumerator FlashRed()
    {
        if (flashImage == null) yield break;
        crackSFX.Play();
        flashImage.color = flashColor;

        while (flashImage.color.a > 0)
        {
            flashImage.color = Color.Lerp(flashImage.color, Color.clear, Time.deltaTime * flashFadeSpeed);
            yield return null;
        }

        flashImage.color = Color.clear;
    }
}
