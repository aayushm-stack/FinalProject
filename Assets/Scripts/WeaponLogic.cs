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
    

    private float autoHideTime=3f;

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
            
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            Debug.Log("Hit: " + hit.transform.name);
            if (enemy != null)
            {
                enemy.TakeDamage(damage); 
                
            }
        }
    }
    void Fire()
    {
        
        if (currentAmmo > 0)
        {
            PlayerShoot();
            currentAmmo--;
            UpdateAmmoUI();
        }
        else
        {
            ShowMessage();
        }
    }
    void ShowMessage()
    {
        if (OutOfAmmoText != null)
        {
            OutOfAmmoText.text = "Out of Ammunition !";
            OutOfAmmoText.enabled = true;
            Invoke("HideMessage", autoHideTime);
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
            ammoText.text = "Ammo Count: " + currentAmmo;
        }
    }
}
