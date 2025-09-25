using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessCamera : MonoBehaviour
{
    [SerializeField] RenderTexture renderTex;
    [SerializeField] Material mat;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, renderTex);
        Graphics.Blit(renderTex, destination, mat);
    }
}
