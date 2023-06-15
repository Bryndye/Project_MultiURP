using System;
using System.Collections;
using System.Collections.Generic;
using Unity.BossRoom.Infrastructure;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemsSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab; // avoir la ref de l'objet pour le pool

    private Action spawnActionItem;
    private void Start()
    {
        spawnActionItem = SpanwItemStart;
        NetworkManager.Singleton.OnServerStarted += spawnActionItem;
    }

    private void SpanwItemStart()
    {
        NetworkManager.Singleton.OnServerStarted -= SpanwItemStart;
        NetworkObjectPool.Singleton.OnNetworkSpawn();
        for (int i = 0; i < 5; i++)
        {
            SpawnItem();
        }
    }

    private void SpawnItem()
    {
        NetworkObject obj = NetworkObjectPool.Singleton.GetNetworkObject(itemPrefab, GetRandomPositionOnMap(), Quaternion.identity);
        obj.GetComponent<Bonus>().Prefab = itemPrefab;
        if(!obj.IsSpawned) obj.Spawn(true);
    }

    private Vector3 GetRandomPositionOnMap()
    {
        return new Vector3(Random.Range(-5,5),1, Random.Range(-5, 5));
    }
}
