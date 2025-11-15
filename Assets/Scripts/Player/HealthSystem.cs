using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;
    
    [Header("Events")]
    public UnityEvent<float> onHealthChanged;
    public UnityEvent onDeath;
    
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        
        onHealthChanged?.Invoke(currentHealth / maxHealth);
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        onHealthChanged?.Invoke(currentHealth / maxHealth);
    }
    
    void Die()
    {
        onDeath?.Invoke();
    }
    
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(1f);
    }
}

