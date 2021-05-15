using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackCameraController : MonoBehaviour
{
    public static BackCameraController instance;
	
	private RenderTexture MaskImage;
	public Texture2D MaskTexture;
	public bool tmp;
	
	Texture2D toTexture2D(RenderTexture rTex){
		if(rTex == null) return MaskTexture;
		RenderTexture.active = rTex;
		int w = rTex.width;
		int h = rTex.height;
		Texture2D tex = new Texture2D(w, h, TextureFormat.RGB24, false);
		// ReadPixels looks at the active RenderTexture.
		tex.ReadPixels(new Rect(0, 0, w, h), 0, 0);
		tex.Apply();
		return tex;
	}
	
	public void CreateMaskImage(){
		Destroy (MaskTexture);
		GetComponent<Camera>().Render();
		MaskTexture = toTexture2D(MaskImage);
	}
	
	void OnPreRender(){
		BackCameraController.instance.CreateMaskImage();
	}
	
	void Start()
    {
        if (BackCameraController.instance) Destroy(gameObject);
        else BackCameraController.instance = this;
		DontDestroyOnLoad(this.gameObject);
		
		MaskImage = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.R8);
		GetComponent<Camera>().targetTexture = MaskImage;
		tmp = true;
	}
	/*
	void Update(){
		BackCameraController.instance.CreateMaskImage();
	}
	*/
}
