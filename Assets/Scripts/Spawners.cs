using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HelloWorld;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.NetworkVariable.Collections;
using MLAPI.Messaging;
using System.Linq;

public class Spawners : MonoBehaviour
{
	public List<GameObject> spawnPositions;
	
	
	public NetworkVariableInt Readies = new NetworkVariableInt(new NetworkVariableSettings {
		WritePermission=NetworkVariablePermission.ServerOnly,
		ReadPermission=NetworkVariablePermission.Everyone
	});

	public NetworkVariableBool HasBeenShuffled = new NetworkVariableBool(new NetworkVariableSettings {
		WritePermission=NetworkVariablePermission.ServerOnly,
		ReadPermission=NetworkVariablePermission.Everyone
	});

	[ServerRpc]
	void ShuffleSpawnsServerRpc(ServerRpcParams rpcParams = default) {
		if (!HasBeenShuffled.Value) {
			HasBeenShuffled.Value=true;
			spawnPositions=spawnPositions.OrderBy(x => Random.value).ToList();
		}
		Readies.Value++;
	}
	
	[ServerRpc]
	void IAmReadyServerRpc(ServerRpcParams rpcParams = default){
		Readies.Value++;
	}

	// Start is called before the first frame update
	void Start()
    {
		//if(NetworkManager.Singleton.ServerClientId == NetworkManager.Singleton.LocalClientId)
			ShuffleSpawnsServerRpc();
		//else
		//	IAmReadyServerRpc();

		if (NetworkManager.Singleton.ConnectedClients.TryGetValue(NetworkManager.Singleton.LocalClientId, out var networkedClient)) {
			var player = networkedClient.PlayerObject.GetComponent<HelloWorldPlayer>();
			if (player) {
				//while (Readies.Value < NetworkManager.Singleton.ConnectedClients.Count()) ;
				while (!HasBeenShuffled.Value) ;
				int i = (int)NetworkManager.Singleton.LocalClientId;
				player.SetPosition(spawnPositions[i].transform.position);
			}
		}

    }
}
