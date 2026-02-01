using JetBrains.Annotations;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 12f, gravity = 9.81f, jumpHeight = 3f;
    
    Vector3 velocity;
    bool isGrounded, isMoving=false;
    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);
    private CharacterController controller;
    public Transform cameraTransform;
    public float health = 100f;
    private int bombsPlanted = 0;
    [Header("UI Settings")]
    public float autoHideTime = 0f;
    public TextMeshProUGUI centerText;
    public AudioSource footstepsAudio;
    [TextArea] public string message = "BOMB PLANTED"; // Custom message for this specific collider

    public int maxAmmo = 100;          // Maximum limit
    public int ammoGainPerCrate = 20;  // How much 1 crate gives

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller=GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;//Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0f) // to attach player to ground
            velocity.y = -2f;
        
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Camera initial --- Vector3 move = transform.right*x + transform.forward*z;

        Vector3 forward = cameraTransform.forward; 
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        Vector3 move = forward.normalized * z + right.normalized * x;
        
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded) 
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);

        velocity.y -= gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        Vector3 horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z);
        if (horizontalVelocity.magnitude > 0f && isGrounded)
            //if (lastPosition != gameObject.transform.position && isGrounded == true)
            isMoving = true;
        else
            isMoving = false;

        lastPosition=gameObject.transform.position;

        if (isMoving)
        {
            footstepsAudio.Play();
        }

        if (bombsPlanted == 2)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Invoke("LoadScene", autoHideTime);
        }
    }
    void LoadScene()
    {
        SceneManager.LoadScene("LastScene");
    }
    public WeaponLogic weapon;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            Debug.Log("Bomb Area Reached!");
            bombsPlanted++;
            ShowMessage();
            Destroy(other.gameObject);
        }
        
        if (other.CompareTag("Ammo"))
        {
            
            Debug.Log("Ammo Crate Collected!");
            weapon.currentAmmo += ammoGainPerCrate;
            if (weapon.currentAmmo > maxAmmo) weapon.currentAmmo = maxAmmo;

            weapon.UpdateAmmoUI();

            Destroy(other.gameObject);
        }
    }
    void ShowMessage()
    {
        if (centerText != null)
        {
            centerText.text = message;
            centerText.enabled = true; 

            if (autoHideTime > 0)
            {
                Invoke("HideMessage", autoHideTime);
            }
        }
    }

    void HideMessage()
    {
        if (centerText != null) centerText.text = "";
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }
    void Die()
    {
        Application.Quit();
    }
}
