using JetBrains.Annotations;
using System.Collections;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;                 // Required for Volumes
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    public float speed = 12f, gravity = 9.81f, jumpHeight = 3f;
    
    Vector3 velocity;
    bool isGrounded;
    //bool isMoving=false;
    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);
    private CharacterController controller;
    public Transform cameraTransform;
    public float health = 100f;
    private int bombsPlanted = 0;
    [Header("UI Settings")]
    public float autoHideTime = 0f;
    public TextMeshProUGUI centerText;
    public TextMeshProUGUI healthText;
    [Header("Footstep Settings")]
    public AudioSource footstepAudio; // Drag your AudioSource here
    public float stepRate = 0.5f;     // Delay between steps (0.5 = walking, 0.3 = running)
    private float nextStepTime = 0f;  // Internal timer
    [TextArea] public string Bomb1msg = "Bomb 1 Location reached !"; // Custom message for this specific collider
    public string Bomb2msg = "Bomb 2 Location reached !";
    public string AmmoMessage = "Ammunition Found !";
    public string HealthPackMessage = "Health Pack Found !";
    public int maxAmmo = 100;          // Maximum limit
    public int maxHealth = 100;
    public int ammoGainPerCrate = 20;  // How much 1 crate gives
    [Header("ScreenFader")]
    public Volume globalVolume;
    private Vignette _vignette;
    public float AnimationDuration=1f;
    [Header("CamerasForCutscene")]
    public GameObject Camera1;
    public GameObject Camera2;
    public GameObject mainCamera;

    [Header("ActorsForCutscene")]
    public GameObject cutscenePlayer1;
    public GameObject cutscenePlayer2;
    public GameObject realPlayer;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (globalVolume.profile.TryGet(out Vignette v))
        {
            _vignette = v;
        }
        controller =GetComponent<CharacterController>();
        UpdateHealthUI();
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
        HandleFootsteps();
        //Vector3 horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z);
        //if (horizontalVelocity.magnitude > 0.01f && isGrounded)
        ////if (lastPosition == gameObject.transform.position && isGrounded == true)
        //    isMoving = true;
        //else
        //    isMoving = false;

        //lastPosition=gameObject.transform.position;
    }
    void Exit() 
    { 

        if (bombsPlanted == 2)
        {
            

            Invoke("LoadSuccessScene", 3f);
        }
    }
    void HandleFootsteps()
    {
        // 1. Get only WASD / Joystick input (Ignoring Mouse X/Y)
        float inputX = Input.GetAxis("Horizontal"); // A / D keys
        float inputZ = Input.GetAxis("Vertical");   // W / S keys

        // 2. Combine them to see if we are pressing anything
        // We use magnitude to check if the combined input is significant
        float inputMagnitude = new Vector2(inputX, inputZ).sqrMagnitude;

        // 3. The Logic Check
        // Condition A: Are we pressing keys? (magnitude > 0.1f)
        // Condition B: Are we on the ground? (isGrounded)
        // Condition C: Has enough time passed? (Time.time >= nextStepTime)

        if (inputMagnitude > 0.1f && isGrounded && Time.time >= nextStepTime)
        {
            // Play the sound
            footstepAudio.Play();

            // Reset the timer
            // "Current Time" + "Delay" = "Time allowed for next step"
            nextStepTime = Time.time + stepRate;
        }
    }
    void LoadSuccessScene()
    {
        SceneManager.LoadScene("LastScene");
    }
    public WeaponLogic weapon;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish1"))
        {
            Debug.Log("Bomb Area 1 Reached!");
            ShowBombMessage(Bomb1msg);
        }
        if (other.CompareTag("Finish2"))
        {
            Debug.Log("Bomb Area 2 Reached!");
            ShowBombMessage(Bomb2msg);
        }
        if (other.CompareTag("Ammo"))
        {
            Debug.Log("Ammo Crate found");
            ShowAmmoMessage();
        }
        if (other.CompareTag("Health"))
        {
            Debug.Log("Health Pack found");
            ShowHealthPackMessage();
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Finish1"))
        {
            
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                bombsPlanted++;
                Destroy(other.gameObject);
                HideMessage();
                BombCutscene1();
            }
            
            
            
        }
        if (other.CompareTag("Finish2"))
        {

            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                bombsPlanted++;
                Destroy(other.gameObject);
                HideMessage();
                BombCutscene2();
            }



        }

        if (other.CompareTag("Ammo"))
        {


            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                weapon.currentAmmo += ammoGainPerCrate;
                Destroy(other.gameObject);
                HideMessage() ;
            }
            if (weapon.currentAmmo > maxAmmo) 
                weapon.currentAmmo = maxAmmo;
            weapon.UpdateAmmoUI();
        }
        if (other.CompareTag("Health"))
        {

            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            {
                health+=20;
                Destroy(other.gameObject);
                HideMessage();
            }
            if (health > maxHealth)
                health = maxHealth;
            UpdateHealthUI();



        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Finish1") || other.CompareTag("Finish2") || other.CompareTag("Ammo") || other.CompareTag("Health"))
        {
            HideMessage();
        }
    }
    async void BombCutscene1()
    {
        mainCamera.SetActive(false);
        realPlayer.SetActive(false);
        Camera1.SetActive(true);
        cutscenePlayer1.SetActive(true);
        //yield return new WaitForSeconds(AnimationDuration);
        await Task.Delay(4000);
        Camera1.SetActive(false);
        cutscenePlayer1.SetActive(false);
        mainCamera.SetActive(true);
        realPlayer.SetActive(true);
        await Task.Delay(1500);
        Exit();
    }
    async void BombCutscene2()
    {
        mainCamera.SetActive(false);
        realPlayer.SetActive(false);
        Camera2.SetActive(true);
        cutscenePlayer2.SetActive(true);
        //yield return new WaitForSeconds(AnimationDuration);
        await Task.Delay(4000);
        Camera2.SetActive(false);
        cutscenePlayer2.SetActive(false);
        mainCamera.SetActive(true);
        realPlayer.SetActive(true);
        await Task.Delay(1500);
        Exit();
    }
    void ShowBombMessage(string msg)
    {
        if (centerText != null)
        {
            centerText.text = msg;
            centerText.enabled = true; 

            
        }
    }

    void HideMessage()
    {
        if (centerText != null) centerText.text = "";
    }
    void ShowAmmoMessage()
    {
        if (centerText != null)
        {
            centerText.text = AmmoMessage;
            centerText.enabled = true;

            
        }
    }
    void ShowHealthPackMessage()
    {
        if (centerText != null)
        {
            centerText.text = HealthPackMessage;
            centerText.enabled = true;


        }
    }


    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            health = 0f;
        }
        UpdateHealthUI();
        if (health == 0f)
        {
            Die();
        }
        
    }
    void Die()
    {
        GetComponent<Player>().enabled = false;
        Camera.main.GetComponent<MouseMovement>().enabled = false;
        GetComponentInChildren<WeaponLogic>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartFadeToBlack();
        Invoke("LoadFailureScene", autoHideTime-1);
    }
    void LoadFailureScene()
    {
        SceneManager.LoadScene("missionFailure");
    }
    public void StartFadeToBlack()
    {
        StartCoroutine(FadeRoutine());
    }

    IEnumerator FadeRoutine()
    {
        float timer = 0;
        
        while (timer < autoHideTime)
        {
            timer += Time.deltaTime;

            // Math to smoothly go from Current Value -> 1.0 (Full Black)
            float newIntensity = Mathf.Lerp(0f, 1.0f, timer / autoHideTime);
            _vignette.intensity.value = newIntensity;

            yield return null; // Wait for next frame
        }

        // Ensure it is perfectly black at the end
        if (_vignette != null)
            _vignette.intensity.value = 1.0f;
        
    }
    public void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Player Health : " + health;
        }
    }
}
