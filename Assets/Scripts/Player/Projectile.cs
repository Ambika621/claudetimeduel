sing UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 15f;
    public GameObject owner;
    public GameObject hitEffectPrefab;
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == owner) return;
        
        HealthSystem health = collision.gameObject.GetComponent<HealthSystem>();
        if (health)
        {
            health.TakeDamage(damage);
        }
        
        if (hitEffectPrefab)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }
        
        Destroy(gameObject);
    }
}

