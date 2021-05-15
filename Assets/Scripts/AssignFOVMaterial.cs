using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignFOVMaterial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		GetComponent<Renderer>().material = new Material( Shader.Find("Unlit/StencilShader") );
		
    }

    // Update is called once per frame
    void OnRenderObject()
    {
		//BackCameraController.instance.CreateMaskImage();
		GetComponent<Renderer>().material.SetTexture("_StencilBuffer", BackCameraController.instance.MaskTexture);
    }
    void OnPostRender(){
		if(!BackCameraController.instance.tmp){
			Destroy( BackCameraController.instance.MaskTexture );
			BackCameraController.instance.tmp = true;
		}
	}
}
