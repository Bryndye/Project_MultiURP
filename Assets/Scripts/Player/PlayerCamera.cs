using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : NetworkBehaviour
{
    [SerializeField] Transform cameraT;
    public Vector2 SensivityMouse = new Vector2(5, 5);
    public Vector2 LimitCameraView = new Vector2(-80, 80);
    private float rotY;
    private float rotX;

    private void Awake()
    {
        if (cameraT == null)
        {
            cameraT = Camera.main.transform;
        }
    }


    private void Start()
    {
        initializeVisualNetwork(); // necessite setup Network Object
    }

    public void LookMouse(InputAction.CallbackContext context)
    {
        if (!IsOwner) return;

        Vector2 _inputMouse = context.ReadValue<Vector2>();
        float x = _inputMouse.x * Time.deltaTime * SensivityMouse.x;
        float y = _inputMouse.y * Time.deltaTime * SensivityMouse.y;

        rotY += x;
        rotX -= y;
        rotX = Mathf.Clamp(rotX, LimitCameraView.x, LimitCameraView.y);

        cameraT.rotation = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = Quaternion.Euler(0, rotY, 0);
    }

    [Header("Visuals Parameters")]
    [SerializeField] private SkinnedMeshRenderer[] body;
    [SerializeField] private GameObject[] FP_objects;
    private void initializeVisualNetwork()
    {
        if (IsOwner)
        {
            foreach (var part in body)
            {
                part.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            }
        }
        else
        {
            foreach (var part in FP_objects)
            {
                part.SetActive(false);
            }
        }
    }
}
