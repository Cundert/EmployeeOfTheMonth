using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackCameraController : MonoBehaviour
{
    public static BackCameraController instance;
	
	public RenderTexture MaskImage;
	
	public void CreateMaskImage(){
		GetComponent<Camera>().Render();
	}
	
	void Start()
    {
        if (BackCameraController.instance) Destroy(gameObject);
        else BackCameraController.instance = this;
		DontDestroyOnLoad(this.gameObject);
		
		MaskImage = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.R8);
		GetComponent<Camera>().targetTexture = MaskImage;
	}
}
