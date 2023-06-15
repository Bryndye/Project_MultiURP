using Unity.Netcode;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    private Vector2 inputMovement;
    [SerializeField] private ushort speedWalk = 10;
    [SerializeField] private ushort speedRun = 15;
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


        Vector3 movement = new Vector3(inputMovement.x, 0, inputMovement.y) * Time.deltaTime * speedWalk;
        transform.Translate(movement, Space.Self);
    }

    private void FixedUpdate()
    {
        
    }

    #region Handle
    public void InputMovement(InputAction.CallbackContext context)
    {
        inputMovement = context.ReadValue<Vector2>();
    }

    public void InputSprint(InputAction.CallbackContext context)
    {
        //
    }

    public void InputJump(InputAction.CallbackContext context)
    {
        //
    }

    public void InputCrouch(InputAction.CallbackContext context)
    {
        //
    }
    #endregion
}
