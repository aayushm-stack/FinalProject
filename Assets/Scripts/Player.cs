using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float speed = 12f, gravity = 9.81f, jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded, isMoving;
    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);
    private CharacterController controller;
    public Transform cameraTransform;
    public float health = 100f;

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

        //Vector3 move = transform.right*x + transform.forward*z;

        Vector3 forward = cameraTransform.forward; // to move in direction of camera view
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        Vector3 move = forward.normalized * z + right.normalized * x;
        
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)  //appyling jump to player body
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);

        velocity.y -= gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (lastPosition != gameObject.transform.position && isGrounded == true)
            isMoving = true;
        else
            isMoving = false;

        lastPosition=gameObject.transform.position;

        if (isMoving)
        {

        }
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
