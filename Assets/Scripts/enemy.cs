using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float health = 100f;

    [Header("References")]
    public Animator animator;

    private bool isDead = false;
    private Collider enemyCollider; 

    void Start()
    {
        enemyCollider = GetComponent<Collider>();
    }

    void Update()
    {
        // 1. HARD STOP: If dead, do absolutely nothing.
        if (isDead) return;

        //float distance = Vector3.Distance(transform.position, player.position);

        //if (distance <= detectionRange)
        //{
        //    FacePlayer();

        //    shotTimer -= Time.deltaTime;
        //    if (shotTimer <= 0f)
        //    {
        //        ShootAtPlayer();
        //        shotTimer = timeBetweenShots;
        //    }
        //}
    }

    //void FacePlayer()
    //{
    //    Vector3 direction = player.transform.position - firePoint.transform.position;
    //    direction.y = 0;

    //    float targetAngle = Quaternion.LookRotation(direction).eulerAngles.y;
    //    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, smoothTime);
    //    transform.rotation = Quaternion.Euler(0f, angle, 0f);
        
    //}

    //void ShootAtPlayer()
    //{
    //    // Fire only if not dead
    //    if (isDead) return;

    //    animator.SetTrigger("Shoot");

    //    // Raycast logic
    //    Vector3 origin = (firePoint != null) ? firePoint.transform.position : transform.position; //+ Vector3.up * 1.5f;
    //    Vector3 dir = (player.transform.position - origin).normalized;

    //    RaycastHit hit;
    //    if (Physics.Raycast(origin, dir, out hit, detectionRange))
    //    {
    //        // Assuming your Player has a script named "PlayerHealth"
    //        // Change "PlayerHealth" to whatever your script is actually named
    //        var target = hit.transform.GetComponent<Player>();
    //        if (target != null)
    //        {
    //            target.TakeDamage(damage);
    //        }
    //    }
    //}

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;
        Debug.Log("Enemy Hit! Health: " + health);

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        if (enemyCollider != null) enemyCollider.enabled = false;
        this.enabled = false;
    }
}