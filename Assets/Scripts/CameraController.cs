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
	
	

	Texture2D toTexture2D(RenderTexture rTex){
		if(rTex == null) return StencilImage;
		RenderTexture.active = rTex;
		int w = rTex.width;
		int h = rTex.height;
		Texture2D tex = new Texture2D(w, h, TextureFormat.RGB24, false);
		// ReadPixels looks at the active RenderTexture.
		tex.ReadPixels(new Rect(0, 0, w, h), 0, 0);
		tex.Apply();
		return tex;
	}

	
	void OnRenderImage (RenderTexture source, RenderTexture destination)
    {
		BackCameraController.instance.CreateMaskImage();
		//destination = BackCameraController.instance.MaskImage;
		//return;
		StencilImage = toTexture2D(BackCameraController.instance.MaskImage);
		
        material.SetTexture("_StencilBuffer", StencilImage);
        Graphics.Blit (source, destination, material);
        Destroy (StencilImage);
    }
	
}
