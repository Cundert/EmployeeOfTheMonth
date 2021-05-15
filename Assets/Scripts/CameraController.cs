using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	
    public static CameraController instance;
    private Material material;
    Texture2D StencilImage;

	void Start()
    {
        if (CameraController.instance) Destroy(gameObject);
        else CameraController.instance = this;
		DontDestroyOnLoad(this.gameObject);
	}
	
	void Awake ()
    {
        material = new Material( Shader.Find("Unlit/StencilShader") );
    }
	
	

	

	/*
	void OnRenderImage (RenderTexture source, RenderTexture destination)
    {
		BackCameraController.instance.CreateMaskImage();
        material.SetTexture("_StencilBuffer", BackCameraController.instance.MaskTexture);
        Graphics.Blit (source, destination, material);
        
    }
    */
	
}
