using HelloWorld;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : NetworkBehaviour
{
	public EquipableItem item;

	public EquipableItem[] allItems;

	public NetworkVariableInt itemId = new NetworkVariableInt(new NetworkVariableSettings {
		WritePermission=NetworkVariablePermission.ServerOnly,
		ReadPermission=NetworkVariablePermission.Everyone
	});

	public void DestroyItem() {
		Destroy(gameObject);
	}

	public void PickItem(HelloWorldPlayer player) {
		item.ChangeStats(player);
	}

	[ServerRpc]
	public void ChangeItemServerRpc(int id) {
		itemId.Value=id;
	}

	public void Update() {
		item=allItems[itemId.Value];
		GetComponent<SpriteRenderer>().sprite=item.image;
	}
}
