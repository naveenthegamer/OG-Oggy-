using UnityEngine;
using System.Collections;

public class HPShake : MonoBehaviour
{
    public float shakeAmount = 5f;     // how strong
    public float shakeDuration = 0.2f; // how long each shake
    public float interval = 1.5f;      // time between shakes

    Vector3 originalPos;

    void Start()
    {
        originalPos = transform.localPosition;
        StartCoroutine(ShakeLoop());
    }

    IEnumerator ShakeLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            yield return StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        float elapsed = 0f;
        originalPos = transform.localPosition;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeAmount;
            float y = Random.Range(-1f, 1f) * shakeAmount;

            transform.localPosition = originalPos + new Vector3( x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
