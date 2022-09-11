using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    public List<BodyPart> body = new List<BodyPart>();
    private float defaultSize = 0.08f;
    private float headSize = 0.1f;
    private int madonPituus = 3;

    void Start()
    {
        body.Add(new BodyPart()
        {
            position = Vector3.zero,
            color = new Vector3(1, 0, 0),
            size = headSize
        });
        for (int i = 1; i <= madonPituus; i++)
        {
            body.Add(new BodyPart()
            {
                position = -2 * i * defaultSize * Vector3.forward,
                color = new Vector3((float)UnityEngine.Random.Range(0, 5) / 4f, (float)UnityEngine.Random.Range(0, 5) / 4f, (float)UnityEngine.Random.Range(0, 5) / 4f),
                size = defaultSize
            });
        }
    }

    internal void Grow()
    {
        body.Add(new BodyPart()
        {
            position = body[body.Count-1].position,
            color = new Vector3((float)UnityEngine.Random.Range(0, 5) / 4f, (float)UnityEngine.Random.Range(0, 5) / 4f, (float)UnityEngine.Random.Range(0, 5) / 4f),
            size = defaultSize
        });
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollisions();
        UpdateBody();
    }

    private void CheckCollisions()
    {
        for (int i = 1; i < body.Count; i++)
        {
            if (i > 0 && dst(body[i].position, transform.position).magnitude < defaultSize)
            {
                Debug.Log("OH NO, collision with ball " + i + " Score: " + body.Count);
                Shrink();
            }
        }
    }

    public void Shrink()
    {
        body.RemoveRange(3, body.Count - 3);
    }

    private void UpdateBody()
    {
        Vector3 prevPos = transform.position;
        body[0] = new BodyPart()
        {
            position = prevPos,
            color = body[0].color,
            size = body[0].size
        };
        for (int i = 1; i < body.Count; i++)
        {
            Vector3 vectorTo = dst(body[i].position, prevPos);
            float dist = dst(body[i].position, prevPos).magnitude;
            if (dist > 2 * defaultSize)
            {
                body[i] = new BodyPart()
                {
                    position = body[i].position + (dist - 2 * defaultSize) * vectorTo.normalized,
                    color = body[i].color,
                    size = body[i].size
                };
            }
            prevPos = body[i].position;
        }
    }

    public Vector3 dst(Vector3 a, Vector3 b)
    {
        float dx = dst(a.x, b.x);
        float dy = dst(a.y, b.y);
        float dz = dst(a.z, b.z);
        return new Vector3(dx, dy, dz);
    }
    private float dst(float a, float b)
    {
        float mod = b - a - Mathf.Floor(b) + Mathf.Floor(a);
        if (mod > 0.5f)
            return mod - 1;
        if (mod < -0.5f)
            return mod + 1;
        return mod;
    }

    public BodyPart[] getBody()
    {
        return body.ToArray();
    }

    public struct BodyPart
    {
        public Vector3 position;
        public Vector3 color;
        public float size;

        public static int GetSize()
        {
            return sizeof(float) * 7;
        }
    }
}
