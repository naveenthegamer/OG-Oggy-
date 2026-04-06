using System;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    public int CurrentHealth;
    public int MaxHealth;
    public PlayerMovement player;
    public event Action<int> OnHealthChanged;
    public Transform respawnpoint;
    public bool isRunning;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
    }
    public void takeDamage(int Damage)
    {
        if (isRunning) return;
        CurrentHealth = Mathf.Max(CurrentHealth-Damage, 0);
        OnHealthChanged?.Invoke(CurrentHealth);
        if (CurrentHealth <= 0) die();
        else 
        {
            StartCoroutine(respawn(respawnpoint));
        }
    }

    public void die()
    {
        Debug.Log("GameOver");
        
    }

    public IEnumerator respawn(Transform CurrentRespawnPoint)
    {   if (isRunning) yield break;
        isRunning = true;
        player.tag = "Robot";//this line could be a potential future error;
        player.CurrentSprite = player.SpriteList[2];
        //Debug.Log("sprite changed");
        yield return new WaitForSeconds(2f);
        //player.rb.linearVelocity = new Vector2(0,0);
        player.tag = "Player";
        player.CurrentSprite = player.SpriteList[1];
        player.transform.position = CurrentRespawnPoint.position;
        player.drainSpeed = player.NormalDrainSpeed;
        player.oxygen = player.maxOxygen;

        isRunning = false;
    }
}
