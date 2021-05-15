using HelloWorld;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
	public EquipableItem item;

	public void PickItem(HelloWorldPlayer player) {
		item.ChangeStats(player);
		Destroy(gameObject);
	}

	public void Start() {
		GetComponent<SpriteRenderer>().sprite=item.image;
	}
}
