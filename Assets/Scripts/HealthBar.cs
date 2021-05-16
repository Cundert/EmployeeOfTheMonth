using HelloWorld;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

	Image image;

	HelloWorldPlayer player;

    // Start is called before the first frame update
    void Start()
    {
		image = GetComponent<Image>();
		// Hierarchy: Player -> Canvas -> HealthBar -> HealthBarInner
		player = gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<HelloWorldPlayer>();
		image.type = Image.Type.Filled;
		image.fillMethod = Image.FillMethod.Horizontal;
		image.fillOrigin = (int) Image.FillMethod.Vertical;
	}

    // Update is called once per frame
    void Update()
    {
		int maxHP = player.maxHp;
		int currentHP = player.HP.Value;
		transform.localScale = new Vector3((float)currentHP / (float)maxHP, transform.localScale.y, transform.localScale.z);
		
	}
}
