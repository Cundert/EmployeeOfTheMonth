using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
	public GameObject source;
	public float BulletSpeed = 10;
	public int BulletDamage = 1;
	public float BulletMaxTime = 2;
	private float BulletAliveTime = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		BulletAliveTime += Time.deltaTime;
		if(BulletAliveTime > BulletMaxTime) Destroy(gameObject);
		
		transform.position += (transform.rotation * new Vector3(1, 0, 0)) * Time.deltaTime * BulletSpeed;
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("Trigger");
		if (other.gameObject.layer == 9)
			Destroy(gameObject);
	}
}
