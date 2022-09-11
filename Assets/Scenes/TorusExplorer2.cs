using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorusExplorer2 : MonoBehaviour
{
    public Camera cam;
    public ComputeShader raymarcher;
    List<ComputeBuffer> buffersToDispose;
    RenderTexture target;
    public GameObject player;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        buffersToDispose = new List<ComputeBuffer>();

        InitRenderTexture();
        CreateScene();

        raymarcher.SetMatrix("_CameraToWorld", cam.cameraToWorldMatrix);
        raymarcher.SetMatrix("_CameraInverseProjection", cam.projectionMatrix.inverse);

        raymarcher.SetTexture(0, "Source", source);
        raymarcher.SetTexture(0, "Target", target);

        int threadGroupsX = Mathf.CeilToInt(cam.pixelWidth / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(cam.pixelHeight / 8.0f);
        raymarcher.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        Graphics.Blit(target, destination);

        foreach (var buffer in buffersToDispose)
        {
            buffer.Dispose();
        }
    }

    void CreateScene()
    {
        PlayerBody.BodyPart[] shapeDataL = player.GetComponent<PlayerBody>().getBody();
        PlayerBody.BodyPart[] shapeData = new PlayerBody.BodyPart[shapeDataL.Length + 2];
        shapeDataL.CopyTo(shapeData,0);
        shapeData[shapeDataL.Length] = GameObject.Find("GameManager").GetComponent<GameManager>().GetTreat();
        shapeData[shapeDataL.Length+1] = GameObject.Find("GameManager").GetComponent<GameManager>().GetBomb();

        ComputeBuffer shapeBuffer = new ComputeBuffer(shapeData.Length, PlayerBody.BodyPart.GetSize());
        shapeBuffer.SetData(shapeData);
        raymarcher.SetBuffer(0, "shapes", shapeBuffer);
        raymarcher.SetInt("numShapes", shapeData.Length);

        buffersToDispose.Add(shapeBuffer);
    }


    void InitRenderTexture()
    {
        if (target == null || target.width != cam.pixelWidth || target.height != cam.pixelHeight)
        {
            if (target != null)
            {
                target.Release();
            }
            target = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            target.enableRandomWrite = true;
            target.Create();
        }
    }
}
