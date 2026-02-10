using System.Collections;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
   
    [SerializeField] Health player;

    //public IEnumerator respawn(Transform CurrentRespawnPoint)
    //{
    //    yield return new WaitForSeconds(2f);
    //    player.tag = "Player";
    //    player.CurrentSprite = player.SpriteList[0];
    //    player.transform.position = CurrentRespawnPoint.position;
    //    player.oxygen = player.maxOxygen;


    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Robot"))
        player.respawnpoint = gameObject.transform;
    }

}
