using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;

	void Start()
    {
        if (CameraController.instance) Destroy(gameObject);
        else CameraController.instance = this;
		DontDestroyOnLoad(this.gameObject);
	}

}
