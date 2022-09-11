using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorusExplorer : MonoBehaviour
{
    public Material mat;
    public float movementSpeed = 0.04f;
    private GameObject cam;

    private void Start()
    {
        cam = new GameObject();
        cam.transform.position = Vector3.zero;
        cam.transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mat.SetVector("_Position", cam.transform.position);
        handelInput();
    }
    private void handelInput()
    {
        Vector3 pos = cam.transform.position;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            pos.y -= movementSpeed;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            pos.y += movementSpeed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            pos.z += movementSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            pos.x -= movementSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            pos.z -= movementSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            pos.x += movementSpeed;
        }
        cam.transform.position = pos;
    }
}
