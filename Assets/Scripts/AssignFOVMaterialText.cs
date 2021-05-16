using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignFOVMaterialText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		
    }
	bool test = true;
    // Update is called once per frame
    void OnRenderObject()
    {
		GetComponent<Renderer>().material.SetTexture("_StencilBuffer", BackCameraController.instance.MaskTexture);
    }
    void OnPostRender(){
		if(!BackCameraController.instance.tmp){
			Destroy( BackCameraController.instance.MaskTexture );
			BackCameraController.instance.tmp = true;
		}
	}
}
