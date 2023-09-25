using UnityEngine;
using UnityEngine.UI;

public class HealthIndicator : MonoBehaviour
{
    public Image healthImage; // Перетащите ваш Image компонент сюда в редакторе
    private float maxHealth = 100;
    public float currentHealth;

    private void Start()
    {
        SetHealth(maxHealth);
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthImage.fillAmount = currentHealth / maxHealth;
    }

    public void SetHealth(float health)
    {
        currentHealth = health;
        healthImage.fillAmount = currentHealth / maxHealth;
    }
}
