using UnityEngine;

public class EnemyScan : MonoBehaviour
{
    [SerializeField] float detectionTime = 1f; // seconds required to catch
    private float timer = 0f;
    private bool playerDetected = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDetected)
        {
            // Increase timer while player is inside
            timer += Time.deltaTime;

            if (timer >= detectionTime)
            {
                Debug.Log("Player has been caught!");
                // Optionally reset timer or trigger catch logic here
            }
        }
        else
        {
            // Lerp timer back down when player leaves
            timer = Mathf.Lerp(timer, 0f, Time.deltaTime * 5f);
            // multiplier controls how quickly it resets
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
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
