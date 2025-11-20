using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RebindInteract : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CameraManager _cameraManager;
    public InputActionReference interactAction;

    private bool _isOpen = false;

    private void Awake()
    {
        if (_cameraManager == null)
        {
            _cameraManager = FindObjectOfType<CameraManager>();
        }
    }

    void OnEnable()
    {
        if (interactAction != null) interactAction.action.Enable();
    }

    void OnDisable()
    {
        if (interactAction != null) interactAction.action.Disable();
    }

    private void Update()
    {
        if (interactAction != null && interactAction.action.WasPressedThisFrame())
        {
            ToggleConversation();
        }
    }

    private void ToggleConversation()
    {
        _isOpen = !_isOpen;

        if (_cameraManager != null)
        {
            _cameraManager.SetConversationView(_isOpen);
        }
        else
        {
            Debug.LogWarning("CameraManager is missing!");
        }
    }
}