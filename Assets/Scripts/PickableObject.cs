using HelloWorld;
using MLAPI.Messaging;
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

	[ClientRpc]
	public void ChangeItemClientRpc(EquipableItem it) {
		item=it;
	}

	public void Start() {
		GetComponent<SpriteRenderer>().sprite=item.image;
	}
}
