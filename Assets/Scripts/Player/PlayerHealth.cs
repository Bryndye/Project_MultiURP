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
    }

    public void RemoveHealth(ushort _damage = 1)
    {
        Health.Value -= _damage;
        if (!IsOwner) return;
        OnHealthChanged?.Invoke(Health.Value); // callBack Interface Health
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
