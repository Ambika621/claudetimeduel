using UnityEngine;

public class CombatController : MonoBehaviour
{
    [Header("Melee Combat")]
    public float meleeRange = 2f;
    public float meleeDamage = 20f;
    public float meleeKnockback = 5f;
    public float attackCooldown = 0.5f;
    
    [Header("Ranged Combat")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 25f;
    public float projectileDamage = 15f;
    
    [Header("VFX")]
    public TrailRenderer attackTrail;
    public ParticleSystem hitEffect;
    
    private float attackTimer;
    private GhostRecorder recorder;
    
    void Start()
    {
        recorder = GetComponent<GhostRecorder>();
    }
    
    void Update()
    {
        attackTimer -= Time.deltaTime;
        
        if (Input.GetMouseButtonDown(0) && attackTimer <= 0)
        {
            PerformMeleeAttack();
        }
        
        if (Input.GetMouseButtonDown(1) && attackTimer <= 0)
        {
            FireProjectile();
        }
    }
    
    void PerformMeleeAttack()
    {
        attackTimer = attackCooldown;
        
        if (recorder) recorder.RecordAction("melee");
        
        if (attackTrail) attackTrail.emitting = true;
        Invoke("StopTrail", 0.2f);
        
        Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward * meleeRange * 0.5f, meleeRange);
        
        foreach (Collider hit in hits)
        {
            if (hit.gameObject == gameObject) continue;
            
            HealthSystem health = hit.GetComponent<HealthSystem>();
            if (health)
            {
                health.TakeDamage(meleeDamage);
                
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb)
                {
                    Vector3 direction = (hit.transform.position - transform.position).normalized;
                    rb.AddForce(direction * meleeKnockback, ForceMode.Impulse);
                }
                
                if (hitEffect) Instantiate(hitEffect, hit.transform.position, Quaternion.identity);
            }
        }
    }
    
    void FireProjectile()
    {
        attackTimer = attackCooldown;
        
        if (recorder) recorder.RecordAction("ranged");
        
        if (projectilePrefab && firePoint)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.velocity = firePoint.forward * projectileSpeed;
            }
            
            Projectile proj = projectile.GetComponent<Projectile>();
            if (proj)
            {
                proj.damage = projectileDamage;
                proj.owner = gameObject;
            }
            
            Destroy(projectile, 5f);
        }
    }
    
    void StopTrail()
    {
        if (attackTrail) attackTrail.emitting = false;
    }
}

