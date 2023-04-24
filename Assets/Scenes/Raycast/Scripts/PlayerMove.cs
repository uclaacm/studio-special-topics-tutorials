using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField, Min(0)] float sensitivity = 1.0f;
    [SerializeField, Min(0)] float speed = 3.0f;
    [SerializeField, Min(0)] float shiftMult = 2.0f;

    void Update()
    {
        Quaternion mouseMotionX = Quaternion.Euler(0, Input.GetAxisRaw("Mouse X") * sensitivity, 0);
        Quaternion mouseMotionY = Quaternion.Euler(-Input.GetAxisRaw("Mouse Y") * sensitivity, 0, 0);

        transform.rotation =  mouseMotionX * transform.rotation * mouseMotionY;

        float speed = this.speed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed *= shiftMult;
        }

        transform.position += Time.deltaTime
            * speed
            * transform.forward
            * Input.GetAxisRaw("Vertical");

        transform.position += Time.deltaTime
            * speed
            * transform.right
            * Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        // If left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
