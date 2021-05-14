using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
		float val = 3*Time.deltaTime;
		Vector2 dir=new Vector2(0, 0);
		if (Input.GetKey("s")) dir+=new Vector2(0, -1);
		if (Input.GetKey("w")) dir+=new Vector2(0, 1);
		if (Input.GetKey("a")) dir+=new Vector2(-1, 0);
		if (Input.GetKey("d")) dir+=new Vector2(1, 0);
		dir.Normalize();
		dir=dir*val;
		transform.position+=new Vector3(dir.x, dir.y, 0);
	}
}
