using System.Collections;
using TMPro;
using UnityEngine;
public class WeaponLogic : MonoBehaviour
{
    
    public Transform hipPosition;
    public Transform adsPosition;

    private bool isAiming;

    public float smoothTime = 0.08f;
    private Vector3 velocity;
    public float FOVvelocity;

    public float hipFOV = 60f;
    public float adsFOV = 30f;
    public AudioSource voiceOverSource;
    //now for shoot
    public float damage = 10f, range = 100f;
    
    public int currentAmmo = 30;       // Starting bullets
    public GameObject muzzleFlash;

    public float MsgAutoHideTime=4f;
    public float fireDelay;
    private float nextShotTime=0f;
    public GameObject crosshair;
    public GameObject dirtImpactPrefab;
    [Header("UI Reference")]
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI OutOfAmmoText;

    void Start()
    {
        UpdateAmmoUI();
        
    }
    void LateUpdate()
    {
        isAiming = Input.GetMouseButton(1);

        Transform target = isAiming ? adsPosition : hipPosition;
        float targetFOV = isAiming ? adsFOV : hipFOV;

        if(isAiming)
            crosshair.SetActive(false);
        if (!isAiming)
            crosshair.SetActive(true);

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
        if (PauseMenu.GameIsPaused) 
            return;
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

    }
    void PlayerShoot()
    {
        //audio
        voiceOverSource.Play();
        RaycastHit hit;
        
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, range))
        {
            StartCoroutine(ShowMuzzleFlash());
            //Enemy enemy = hit.transform.GetComponent<Enemy>();
            Debug.Log("Hit: " + hit.transform.name);
            Enemy enemyScript = hit.transform.GetComponentInParent<Enemy>();
            //if (enemy != null)
            //{
            //    enemy.TakeDamage(damage); 

            //}
            if (hit.transform.CompareTag("Enemy"))
            {
                Debug.Log("Hit: Enemy.");
                // Apply damage code here
                enemyScript.TakeDamage(damage);
            }
            if (hit.transform.CompareTag("EnemyHead"))
            {
                Debug.Log("Hit: Enemy. HeadShot!");
                // Apply damage code here
                ShowMessage("Head Shot !");
                enemyScript.TakeDamage(3*damage);
            }
            if (!(hit.transform.CompareTag("Enemy")|| hit.transform.CompareTag("EnemyHead")))
            {
                // 1. Spawn the dust at the hit point
                // Quaternion.LookRotation(hit.normal) makes the dust shoot OUT of the ground, not sideways.
                GameObject impact = Instantiate(dirtImpactPrefab, hit.point, Quaternion.LookRotation(hit.normal));

                // 2. Clean it up after 2 seconds
                Destroy(impact, 2f);
            }
        }
    }
    public float showDuration = 0.5f;
    IEnumerator ShowMuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(showDuration);
        muzzleFlash.SetActive(false);
    }
    void Fire()
    {
        
        if (currentAmmo > 0)
        {
            if (Time.time >= nextShotTime)
            {
                PlayerShoot();
                currentAmmo--;
                UpdateAmmoUI();
                nextShotTime= Time.time+fireDelay;
            }
        }
        else
        {
            ShowMessage("Out of Ammunition !");
        }
    }
    void ShowMessage(string s)
    {
        if (OutOfAmmoText != null)
        {
            OutOfAmmoText.text = s;
            OutOfAmmoText.enabled = true;
            Invoke("HideMessage", MsgAutoHideTime);
        }
    }
    void HideMessage()
    {
        OutOfAmmoText.enabled=false;
    }
    
    public void UpdateAmmoUI()
    {
        if (ammoText != null)
        { 
            ammoText.text = "Ammo Count : " + currentAmmo;
        }
    }
}
