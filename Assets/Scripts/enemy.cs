using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float health = 100f;

    [Header("References")]
    public Animator animator;

    private bool isDead = false;
    private Collider enemyCollider;

    [Header("Targeting")]
    public Transform player;          // Drag Player here (or finds automatically)
    public float detectionRange = 15f;// How close player must be to start shooting
    public float rotationSpeed = 5f;  // How fast enemy turns

    [Header("Shooting Settings")]
    //public Transform gunPoint;        // The tip of the gun (Raycast origin)
    public float damage = 10f;
    public float visualOffsetAngle = 0f;

    [Header("Animation Sync (Crucial)")]
    public float fireRate = 1.5f;     // Total length of your shoot animation clip
    //public float damageDelay = 0.5f;  // Time from Start of anim -> Recoil/Fire frame

    [Header("Audio")]
    //public AudioClip shootSound;
    public AudioSource audioSource;

    [Header("Sight Settings")]
    public Transform eyePoint; // Drag the Enemy's "Head" bone here (or create an empty object at eye level)
    public float playerHeightOffset = 1.5f; // Aim for the chest, not the feet!

    private float nextFireTime = 0f;

    void Start()
    {
        enemyCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        
    }

    void Update()
    {
        if (isDead) {
            animator.SetBool("isDead", true);
            return; 
        }
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 1. Check if Player is in Range
        if (distanceToPlayer <= detectionRange)
        {
            // 2. Face the Player
            FacePlayer();
            animator.SetBool("isShooting", true);
            // 3. Check Fire Rate
            if (Time.time >= nextFireTime)
            {
                
                ShootSequence();
                nextFireTime = Time.time + fireRate; // Reset Cooldown
            }
        }
        else
        {
            // Optional: Return to Idle if player runs away
            animator.SetBool("isShooting", false);
        }
    }

    // Rotates smoothly towards the player without tilting up/down
    void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Keep the enemy standing upright (don't lean back)

        Quaternion standardLook = Quaternion.LookRotation(direction);
        Quaternion correctedLook = standardLook * Quaternion.Euler(0, visualOffsetAngle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, correctedLook, Time.deltaTime * rotationSpeed);
    }

    // The Sequence that matches your Animation
    void ShootSequence()
    {
        //A.Start Animation
        animator.SetTrigger("Shoot"); // Assuming you use a Trigger. If using Bool, set it true here.

        //B.Wait for the specific "Recoil" frame
   
        //This is the magic line that syncs the damage with the visual kick
   
        //yield return new WaitForSeconds(damageDelay);

        //C.Play Sound
        audioSource.Play();

        //D.Raycast Logic(The actual Shot)
        ShootRaycast();
    }


    void ShootRaycast()
    {
        if (eyePoint == null)
        {
            Debug.LogError("FIX ME: 'Eye Point' is missing on enemy named: " + gameObject.name);
            return; // Stop here so we don't crash
        }
        // 1. Define the Start Point (The Enemy's Eyes)
        Vector3 startPoint = eyePoint.position;

        // 2. Define the End Point (The Player's Chest/Center)
        // We add an offset because 'player.position' is usually at the feet.
        Vector3 targetPoint = player.position + Vector3.up * playerHeightOffset;

        // 3. Calculate Direction
        // Direction = Destination - Origin
        Vector3 shootDirection = (targetPoint - startPoint).normalized;

        RaycastHit hit;

        // 4. Fire the Ray
        if (Physics.Raycast(startPoint, shootDirection, out hit, detectionRange))
        {
            // Debug Line: See the laser in the Scene view
            Debug.DrawLine(startPoint, hit.point, Color.red, 1.0f);

            //if (hit.transform.CompareTag("Player"))
            //{
            //    Debug.Log("Hit Player from Eye Sight!");
            //    // Apply damage code here
            //    hit.transform.GetComponent<Player>().TakeDamage(damage);
            //}
            Player player = hit.transform.GetComponentInParent<Player>();
            
            if (player != null)
            {
                Debug.Log("Hit: Player");
                player.TakeDamage(damage);
            }
        }
    }


    //void ShootRaycast()
    //{
    //    RaycastHit hit;
    //    // Shoot from gunPoint forward
    //    if (Physics.Raycast(gunPoint.position, gunPoint.forward, out hit, detectionRange))
    //    {
    //        // Visualize the laser in Scene view for debugging
    //        Debug.DrawLine(gunPoint.position, hit.point, Color.red, 0.5f);

    //        if (hit.transform.CompareTag("Player"))
    //        {
    //            // Call your existing Damage function on the player
    //            // Example: hit.transform.GetComponent<PlayerHealth>().TakeDamage(damage);
    //            Debug.Log("Hit Player! Dealt " + damage + " damage.");
    //        }
    //    }
    //}




    //void Update()
    //{
    //    // 1. HARD STOP: If dead, do absolutely nothing.
    //    if (isDead) return;

    //    //float distance = Vector3.Distance(transform.position, player.position);

    //    //if (distance <= detectionRange)
    //    //{
    //    //    FacePlayer();

    //    //    shotTimer -= Time.deltaTime;
    //    //    if (shotTimer <= 0f)
    //    //    {
    //    //        ShootAtPlayer();
    //    //        shotTimer = timeBetweenShots;
    //    //    }
    //    //}
    //}

    ////void FacePlayer()
    ////{
    ////    Vector3 direction = player.transform.position - firePoint.transform.position;
    ////    direction.y = 0;

    ////    float targetAngle = Quaternion.LookRotation(direction).eulerAngles.y;
    ////    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, smoothTime);
    ////    transform.rotation = Quaternion.Euler(0f, angle, 0f);

    ////}

    ////void ShootAtPlayer()
    ////{
    ////    // Fire only if not dead
    ////    if (isDead) return;

    ////    animator.SetTrigger("Shoot");

    ////    // Raycast logic
    ////    Vector3 origin = (firePoint != null) ? firePoint.transform.position : transform.position; //+ Vector3.up * 1.5f;
    ////    Vector3 dir = (player.transform.position - origin).normalized;

    ////    RaycastHit hit;
    ////    if (Physics.Raycast(origin, dir, out hit, detectionRange))
    ////    {
    ////        // Assuming your Player has a script named "PlayerHealth"
    ////        // Change "PlayerHealth" to whatever your script is actually named
    ////        var target = hit.transform.GetComponent<Player>();
    ////        if (target != null)
    ////        {
    ////            target.TakeDamage(damage);
    ////        }
    ////    }
    ////}

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;
        if(health > 0)
        animator.SetTrigger("Damage");
        Debug.Log("Enemy Hit! Health: " + health);

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        if (enemyCollider != null) enemyCollider.enabled = false;
        this.enabled = false;
    }
}