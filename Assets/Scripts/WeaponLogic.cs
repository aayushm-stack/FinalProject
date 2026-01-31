using UnityEngine;
using System.Collections;
public class WeaponLogic : MonoBehaviour
{
    //variables for camera movement on right click
    public Transform hipPosition;
    public Transform adsPosition;

    //public float aimSpeed = 10f;
    private bool isAiming;

    public float smoothTime = 0.08f;
    private Vector3 velocity;
    public float FOVvelocity;

    public float hipFOV = 60f;
    public float adsFOV = 30f;

    //now for shoot
    public float damage = 10f, range = 100f;
    //public ParticleSystem muzzleFlash;
    void LateUpdate()
    {
        isAiming = Input.GetMouseButton(1);

        Transform target = isAiming ? adsPosition : hipPosition;
        float targetFOV = isAiming ? adsFOV : hipFOV;

        Camera.main.fieldOfView = Mathf.SmoothDamp(
            Camera.main.fieldOfView,
            targetFOV,
            ref FOVvelocity,
            smoothTime
        );
        transform.localPosition = Vector3.SmoothDamp(
            transform.localPosition,
            target.localPosition,
            ref velocity,
            smoothTime
        );
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayerShoot();
        }

    }
    void PlayerShoot()
    {
        RaycastHit hit;
        // Shoot from the Main Camera's center
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
        {
            // Look for the EnemyAI script on the object we hit
            navEnemy enemy = hit.transform.GetComponent<navEnemy>();
            Debug.Log("Hit: " + hit.transform.name);
            if (enemy != null)
            {
                // Apply damage to the enemy
                enemy.TakeDamage(damage); // Replace 25f with your weapon damage variable

                // Optional: Add impact effect particle here
                
            }
        }
    }

}
