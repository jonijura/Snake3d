using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerBody.BodyPart treat;
    private PlayerBody.BodyPart bomb;
    private PlayerBody pb;
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
        pb = player.GetComponent<PlayerBody>();
        treat = new PlayerBody.BodyPart
        {
            position = new Vector3((float)UnityEngine.Random.Range(0, 10) / 10f, (float)UnityEngine.Random.Range(0, 10) / 10f, (float)UnityEngine.Random.Range(0, 10) / 10f),
            color = new Vector3(1, 1, 1),
            size = 0.01f
        };
        bomb = new PlayerBody.BodyPart
        {
            position = new Vector3((float)UnityEngine.Random.Range(0, 10) / 10f, (float)UnityEngine.Random.Range(0, 10) / 10f, (float)UnityEngine.Random.Range(0, 10) / 10f),
            color = new Vector3(1f, 0,0),
            size = 0.2f
        };
    }
    void Update()
    {
        if (pb.dst(pb.transform.position, treat.position).magnitude < 0.05f)
        {
            pb.Grow();
            treat = new PlayerBody.BodyPart
            {
                position = new Vector3((float)UnityEngine.Random.Range(0, 10) / 10f, (float)UnityEngine.Random.Range(0, 10) / 10f, (float)UnityEngine.Random.Range(0, 10) / 10f),
                color = new Vector3(1, 1, 1),
                size = 0.01f
            };
        }
        else
        {
            treat = new PlayerBody.BodyPart
            {
                position = treat.position + 0.2f*Time.deltaTime*player.transform.up,
                color = new Vector3(1, 1, 1),
                size = 0.01f
            };
        }
        if (pb.dst(pb.transform.position, bomb.position).magnitude < 0.25f)
        {
            bomb = new PlayerBody.BodyPart
            {
                position = new Vector3((float)UnityEngine.Random.Range(0, 10) / 10f, (float)UnityEngine.Random.Range(0, 10) / 10f, (float)UnityEngine.Random.Range(0, 10) / 10f),
                color = bomb.color,
                size = 0.2f
            };
            Debug.Log("KABOOM score:" + pb.getBody().Length);
            pb.Shrink();
        }
    }

    public PlayerBody.BodyPart GetTreat()
    {
        return treat;
    }
    public PlayerBody.BodyPart GetBomb()
    {
        return bomb;
    }
}
