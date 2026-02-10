using System.Collections;
using UnityEngine;

public class PlayerOut : MonoBehaviour
{
    [SerializeField] PlayerMovement player;
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Robot"))
            player.PlayerLose(true);
            
    }

 
}
