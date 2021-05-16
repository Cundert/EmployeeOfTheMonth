using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
	public GameObject[] pickableObjectPrefabList;

	public float timeSinceLastSpawn = 0;
	public float spawnRate = 15;

    // Start is called before the first frame update
    void Start()
    {

    }

	void SpawnSingleItem(GameObject itemPrefab, Vector3 position)
	{
		GameObject newPickableObject = Instantiate(itemPrefab, position, Quaternion.identity);
		newPickableObject.GetComponent<NetworkObject>().Spawn();
	}

	void SpawnItemBatch()
	{
		List<int> chosenColumns = new List<int>();
		for(int i = 0; i < 3; i++)
		{
			int itemIndex = Random.Range(0, pickableObjectPrefabList.Length);

			int selectedColumn = Random.Range(0, 4);
			while (chosenColumns.Contains(selectedColumn))
				selectedColumn = Random.Range(0, 4);
			chosenColumns.Add(selectedColumn);

			GameObject selectedSpawnPoint = transform.Find(string.Format("SpawnPoints/{0}-{1}", i + 1, selectedColumn + 1)).gameObject;
			SpawnSingleItem(pickableObjectPrefabList[itemIndex], selectedSpawnPoint.transform.position);
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
