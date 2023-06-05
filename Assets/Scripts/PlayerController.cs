using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private Vector2 inputMovement;

    public override void OnNetworkSpawn() // INIT gameObject when spawn on network
    { 
        base.OnNetworkSpawn();
    }

    private void Update()
    {
        if (!Application.isFocused || ! IsOwner)
        {
            return;
        }
        inputMovement.x = Input.GetAxis("Horizontal");
        inputMovement.y = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(inputMovement.x, 0, inputMovement.y) * Time.deltaTime;
        transform.Translate(movement, Space.Self);
    }

    private void FixedUpdate()
    {
        
    }
}
