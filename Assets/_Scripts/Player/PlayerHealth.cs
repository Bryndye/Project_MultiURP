using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using JetBrains.Annotations;

public class PlayerHealth : NetworkBehaviour
{
    public NetworkVariable<ushort> Health = new(100, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    [SerializeField] private ushort maxHealth = 100;
    [CanBeNull] public static event Action <ushort> OnHealthChanged;

    private void Start()
    {
        if (IsOwner)
        {
            Health.Value = maxHealth;
            OnHealthChanged?.Invoke(Health.Value);
            //Health.OnValueChanged += HealthChanged; // Peut etre mis ici et ne pas avoir la verification dans fct AddHealth/ RemoveHealth de isOwner
        }

        if (!IsServer) Health.OnValueChanged += HealthChanged; // callBack si Health changed
    }


    private void HealthChanged(ushort _previousValue, ushort _newValue)
    {
        Debug.Log("Health Changed Callback");
        if (!IsOwner) return;
        OnHealthChanged?.Invoke(_newValue); // callBack Interface Health
    }


    // Call from the server
    public void AddHealth(ushort _heal = 1)
    {
        Health.Value += _heal;
        if (!IsOwner) return;
        OnHealthChanged?.Invoke(Health.Value); // callBack Interface Health
        ClientMusicPlayer.Instance.PlayAudioClipByName();
    }

    public void RemoveHealth(ushort _damage = 1)
    {
        Health.Value -= _damage;
        if (!IsOwner) return;
        OnHealthChanged?.Invoke(Health.Value); // callBack Interface Health
        ClientMusicPlayer.Instance.PlayAudioClipByName();
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerHealth _playerHealth))
        {
            var playerData1 = new PlayerData()
            {
                Id = OwnerClientId,
                Health = Health.Value
            };
            var playerData2 = new PlayerData()
            {
                Id = _playerHealth.OwnerClientId,
                Health = _playerHealth.Health.Value
            };
            TestNetworkVoidServerRpc(playerData1, playerData2);
        }
    }

    struct PlayerData : INetworkSerializable
    {
        public ulong Id;
        public ushort Health;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Id);
            serializer.SerializeValue(ref Health);
        }
    }

    [ServerRpc(RequireOwnership = true)]
    private void TestNetworkVoidServerRpc(PlayerData _player1, PlayerData _player2)
    {
        if (_player2.Health > _player1.Health)
        {
            Debug.Log(_player2.Id + " win");
        }
        else
        {
            Debug.Log(_player1.Id + " win");
        }
    }

    [ClientRpc]
    private void hasKilledClientRpc(ClientRpcParams clientRpcParams = default)
    {
        if (!IsOwner) return;
        Debug.Log("You win the comparaison Health");
    }

    [ClientRpc]
    private void wasKilledClientRpc(ClientRpcParams clientRpcParams = default)
    {
        if (!IsOwner) return;
        Debug.Log("You lose the comparaison Health");
        // event dead screen
        //NetworkManager.Singleton.Shutdown(); // permet de couper la connexion au joueur a la game
    }

    #region Editor fcts
    // Call from the server
    [ContextMenu("Add health")]
    public void AddHealthEditor()
    {
        AddHealth(10);
    }
    [ContextMenu("Remove health")]
    public void RemoveHealthEditor()
    {
        RemoveHealth(10);
    }
    #endregion
}
