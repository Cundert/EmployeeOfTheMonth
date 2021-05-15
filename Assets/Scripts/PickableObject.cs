using HelloWorld;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
	public EquipableItem item;

	public void DestroyItem() {
		Destroy(gameObject);
	}

	public void PickItem(HelloWorldPlayer player) {
		item.ChangeStats(player);
	}

	public void Start() {
		GetComponent<SpriteRenderer>().sprite=item.image;
	}
}
