using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class navEnemy : MonoBehaviour
{
    [Header("Stats")]
    public float health = 100f;
    public float damage = 10f;

    [Header("Movement Settings")]
    public float detectionRange = 20f;
    public float shootingRange = 10f;
    public float rotationSpeed = 5f; // How fast he turns to face you

    [Header("Shooting Settings")]
    public float timeBetweenShots = 1.5f; // Set this to match your animation length!
    public Transform firePoint; // Assign a localized point (gun barrel) or use transform position

    [Header("References")]
    public Transform player;

    // Internal variables
    private NavMeshAgent agent;
    private Animator animator;
    private float shotTimer;
    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // IMPORTANT: Unlink agent rotation from movement so we can control facing direction manually when shooting
        agent.updateRotation = true;
        agent.updateUpAxis = true;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // --- STATE 1: IDLE / LOST ---
        if (distanceToPlayer > detectionRange)
        {
            StopEnemy();
        }
        // --- STATE 2: CHASE ---
        else if (distanceToPlayer > shootingRange && distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
        }
        // --- STATE 3: ATTACK ---
        else if (distanceToPlayer <= shootingRange)
        {
            AttackPlayer();
        }
    }

    void StopEnemy()
    {
        agent.isStopped = true;
        animator.SetBool("IsWalking", false);
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        animator.SetBool("IsWalking", true);

        // Let the NavMeshAgent handle rotation while moving
        agent.updateRotation = true;
    }

    void AttackPlayer()
    {
        // 1. Stop Moving
        agent.isStopped = true;
        animator.SetBool("IsWalking", false);

        // 2. Face the Player Smoothly (Dynamic Rotation)
        FaceTarget();

        // 3. Handle Shooting Timer
        shotTimer -= Time.deltaTime;

        if (shotTimer <= 0f)
        {
            Shoot();
            shotTimer = timeBetweenShots; // Reset timer based on animation length
        }
    }

    void FaceTarget()
    {
        // Get direction to player
        Vector3 direction = (player.position - transform.position).normalized;

        // Flatten the direction (so enemy doesn't look up/down, only left/right)
        direction.y = 0;

        // Create the rotation
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            // Smoothly interpolate to that rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void Shoot()
    {
        // Trigger Animation
        animator.SetTrigger("Shoot");

        // Raycast Logic
        Vector3 origin = firePoint != null ? firePoint.position : transform.position + Vector3.up;
        Vector3 dir = (player.position - origin).normalized;

        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit, shootingRange))
        {
            // Check if we hit the player
            // Make sure your Player object has the tag "Player" or checks the script component
            Player playerHealth = hit.transform.GetComponent<Player>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Enemy hit the player!");
            }
        }
    }

    // --- PUBLIC METHODS FOR PLAYER TO CALL ---

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;

        // Optional: Play a "Get Hit" animation here

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // Stop all movement
        agent.isStopped = true;
        agent.velocity = Vector3.zero;

        // Disable the agent so other enemies can walk over the body
        agent.enabled = false;

        // Disable the collider so the player can't shoot the dead body anymore
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // Play Death Animation
        animator.SetTrigger("Die");

        // We do NOT Destroy(gameObject) so the body stays on the floor
    }
}