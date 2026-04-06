using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image[] hearts;       
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;
    [SerializeField] private Health health;
    

    void Start()
    {
        health.OnHealthChanged += UpdateHearts;
        //UpdateHearts(health.CurrentHealth, health.MaxHealth);
    }

    public void UpdateHearts(int current)
    {
        for (int i = 0; i < hearts.Length; i++)
        //hearts[i].sprite = i < current ? fullHeart : emptyHeart;
        {
            if (i < current)
            {
                hearts[i].sprite = fullHeart;
            }
            else { hearts[i].sprite = emptyHeart; }
        }
    }
}
