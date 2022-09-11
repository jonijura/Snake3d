using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explorer : MonoBehaviour
{
    public Material mat;
    public Vector2 pos;
    public float scale = 1;
    public float movSpd = 0.01f;
    public Vector4 colors = new Vector4(.3f, .45f, .6f, .9f);
    public float animat = 0;
    void FixedUpdate()
    {
        float aspect = (float)Screen.width / (float)Screen.height;
        mat.SetVector("_Area", new Vector4(pos.x, pos.y, scale*aspect, scale));
        mat.SetVector("_ColorGradient", colors);
        mat.SetFloat("_ColorModifier", animat);
        handelInput();
    }

    private void handelInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            scale *= 1.1f;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            scale *= 0.9f;
        }        
        if (Input.GetKey(KeyCode.W))
        {
            pos.y += movSpd * scale;
        }        
        if (Input.GetKey(KeyCode.A))
        {
            pos.x -= movSpd * scale;
        }        
        if (Input.GetKey(KeyCode.S))
        {
            pos.y -= movSpd * scale;
        }        
        if (Input.GetKey(KeyCode.D))
        {
            pos.x += movSpd * scale;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            animat += 0.01f;
        }


    }
}
