using UnityEngine;
using System.Collections;

public class ScanSensorTimedFade : MonoBehaviour
{
    public string requiredTag = "Scan";
    public float activeAlpha = 1f;
    public float fadeSpeed = 3f;
    public float holdTime = 1.5f; // how long it stays opaque

    SpriteRenderer sr;
    float initialAlpha;
    Coroutine fadeRoutine;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        initialAlpha = sr.color.a;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(requiredTag)) return;

        // Stop any running fade
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        // 1️⃣ Fade in
        yield return FadeTo(activeAlpha);

        // 2️⃣ Hold
        yield return new WaitForSeconds(holdTime);

        // 3️⃣ Fade back
        yield return FadeTo(initialAlpha);
    }

    IEnumerator FadeTo(float targetAlpha)
    {
        while (!Mathf.Approximately(sr.color.a, targetAlpha))
        {
            Color c = sr.color;
            c.a = Mathf.MoveTowards(c.a, targetAlpha, fadeSpeed * Time.deltaTime);
            sr.color = c;
            yield return null;
        }
    }
}
