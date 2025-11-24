using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessCamera : MonoBehaviour
{
    [SerializeField] RenderTexture renderTex;
    [SerializeField] Material mat;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //Graphics.Blit(source, renderTex, mat);
        //Graphics.Blit(renderTex, destination);

        Graphics.Blit(source, destination, mat);
    }
}
