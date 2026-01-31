//using UnityEngine;
//using System.Collections;

//public class MouseMovement : MonoBehaviour
//{
//    public float mouseSensitivity = 100f, topClamp = -90f, bottomClamp = 90f;
//    float xRotation = 0f, yRotation = 0f;
//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        Cursor.lockState=CursorLockMode.Locked;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
//        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
//        xRotation -= mouseY;
//        xRotation=Mathf.Clamp(xRotation, topClamp, bottomClamp);
//        yRotation += mouseX;
//        transform.localRotation=Quaternion.Euler(xRotation, yRotation, 0f);
//    }
//}
using UnityEngine;
using System.Collections;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertical look (camera only)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal look (player body only)
        playerBody.Rotate(0f, mouseX, 0f);
    }
}
