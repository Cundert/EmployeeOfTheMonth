using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : NetworkBehaviour
{
	public GameObject pickableObjectPrefab;
	public int amountOfItems;

	public float timeSinceLastSpawn = 0;
	public float spawnRate = 15;

    // Start is called before the first frame update
    void Start()
    {

    }

	void SpawnSingleItem(int id, Vector3 position)
	{
		GameObject newPickableObject = Instantiate(pickableObjectPrefab, position, Quaternion.identity);
		newPickableObject.GetComponent<PickableObject>().ChangeItemServerRpc(id);
		newPickableObject.GetComponent<NetworkObject>().Spawn();
	}

	void SpawnItemBatch()
	{
		List<int> chosenColumns = new List<int>();
		for(int i = 0; i < 3; i++)
		{
			int itemIndex = Random.Range(0, amountOfItems);

			int selectedColumn = Random.Range(0, 4);
			while (chosenColumns.Contains(selectedColumn))
				selectedColumn = Random.Range(0, 4);
			chosenColumns.Add(selectedColumn);

			GameObject selectedSpawnPoint = transform.Find(string.Format("SpawnPoints/{0}-{1}", i + 1, selectedColumn + 1)).gameObject;
			SpawnSingleItem(itemIndex, selectedSpawnPoint.transform.position);
		}

	}

    // Update is called once per frame
    void Update()
    {
		if (NetworkManager.Singleton.IsServer) {
			timeSinceLastSpawn += Time.deltaTime;
			if (timeSinceLastSpawn > spawnRate)
			{
				SpawnItemBatch();
				timeSinceLastSpawn -= spawnRate;
			}
		}
	}
}
