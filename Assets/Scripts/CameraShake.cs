using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class CameraShake : MonoBehaviour
{

    [Header("Shaker")]
    public ShakeData cameraShake;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.CompareTag("zone"))
        {
            CameraShakerHandler.Shake(cameraShake);
        }
    }
    void Update()
    {
        
    }
}
