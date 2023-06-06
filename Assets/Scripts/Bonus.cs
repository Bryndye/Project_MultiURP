using System.Collections;
using System.Collections.Generic;
using Unity.BossRoom.Infrastructure;
using Unity.Netcode;
using UnityEngine;

public class Bonus : NetworkBehaviour
{
    public GameObject Prefab;
    private void OnTriggerEnter(Collider other)
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            return;
        }

        if (other.TryGetComponent(out PlayerNetworkTest _scorePlayer))
        {
            _scorePlayer.AddScore(5);
        }

        NetworkObject.Despawn(); // Dispawn THIS gameObject & return to the poolSystem
        //NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, Prefab); // return the gameObject to the poolSystem
    }
}
