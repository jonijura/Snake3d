using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 0.1f;
    [SerializeField] private float sensX = 1f;
    [SerializeField] private float sensY = 1f;
    [SerializeField] private float rotationSpeed = 1f;
    // Start is called before the first frame update
    void Update()
    {
        handleKeyboardInput();
        handleMouseInput();
    }

    private void handleMouseInput()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        transform.Rotate(Vector3.up * mouseX + Vector3.left*mouseY);
        
    }

    private void handleKeyboardInput()
    {
        Vector3 pos = gameObject.transform.position;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            pos -= movementSpeed * Time.deltaTime*transform.up;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            pos += movementSpeed * Time.deltaTime * transform.up;
        }
        if (Input.GetKey(KeyCode.W))
        {
            pos += movementSpeed * Time.deltaTime*transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            pos -= movementSpeed * Time.deltaTime*transform.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            pos -= movementSpeed * Time.deltaTime * transform.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            pos += movementSpeed * Time.deltaTime * transform.right;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.forward*rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        gameObject.transform.position = pos;
    }
}
