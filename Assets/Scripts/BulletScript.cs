using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		float a = transform.GetChild(0).GetComponent<SpriteRenderer>().color.a;
		a -= 2* Time.deltaTime;
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, a);
        if(a <= 0) Destroy(gameObject);
    }
}
