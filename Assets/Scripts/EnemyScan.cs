using UnityEngine;

public class EnemyScan : MonoBehaviour
{
    [SerializeField] float detectionTime; // seconds required to catch
    private float timer = 0f;
    private bool playerDetected = false;
    public PlayerMovement player;
    float t;

    [Header("Visual Feedback")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Color startColor;// = Color.white;
    [SerializeField] Color caughtColor;// = Color.red;


    void Update()
    {
        if (playerDetected)
        {
            // Increase timer while player is inside
            timer += Time.deltaTime;

            if (timer >= detectionTime)
            {
                Debug.Log("Player has been caught!");
                player.PlayerLose(true);
            }
        }
        else
        {
           
            timer = Mathf.Lerp(timer, 0f, Time.deltaTime * 5f);
            // multiplier controls how quickly it resets
        }
        // Update sprite color based on timer progress
        t = Mathf.Clamp01(timer / detectionTime);
        spriteRenderer.color = Color.Lerp(startColor, caughtColor, t);

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDetected = true;
            Debug.Log("Player detected");
        }
        else if ( collision.CompareTag("Robot"))
        {
            playerDetected = false;
            Debug.Log("Robot detected");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDetected = false;
            Debug.Log("Player left detection area");
        }
    }

}
