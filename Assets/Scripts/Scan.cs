using System.Collections;
using UnityEngine;

public class Scan : MonoBehaviour
{
    public Animator animator;
    public string stateName = "Scan";
    public float animationLength = 1f;
    public float waitTime = 2f;
    public AudioSource audioSource;
    

    void Start()
    {
        StartCoroutine(AnimationLoop());
    }

    IEnumerator AnimationLoop()
    {
        while (true)
        {
            animator.Play(stateName, 0, 0f);
            audioSource.Play();
            yield return new WaitForSeconds(animationLength);

            animator.Play(stateName, 0, 0f);
            audioSource.Play();
            yield return new WaitForSeconds(animationLength);

            yield return new WaitForSeconds(waitTime);
        }
    }
}
