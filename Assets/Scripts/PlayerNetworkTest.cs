using System.Collections.Generic;
using Unity.Netcode;
using System.Collections;
using UnityEngine;

public class PlayerNetworkTest : NetworkBehaviour
{
    public NetworkVariable<ushort> Score = new (0,NetworkVariableReadPermission.Everyone ,NetworkVariableWritePermission.Server);

    private void Update()
    {

    }

    // Call from the server
    [ContextMenu("Add Score")]
    public void AddScore(ushort _score = 1)
    {
        Score.Value += _score;
    }
}
