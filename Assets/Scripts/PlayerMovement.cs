using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input System")]
    public InputActionReference moveAction;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("References")]
    public CameraManager cameraManager;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private bool _isOpen = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void OnEnable()
    {
        if (moveAction != null) moveAction.action.Enable();
    }

    void OnDisable()
    {
        if (moveAction != null) moveAction.action.Disable();
    }

    void Update()
    {
        HandleInput();
        HandleInteract();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void HandleInput()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();

        moveDirection = new Vector3(input.x, 0f, input.y);

        if (moveDirection.sqrMagnitude > 1)
        {
            moveDirection.Normalize();
        }
    }

    void HandleInteract()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            _isOpen = !_isOpen;
            Debug.Log("E key pressed! Activating conversation view.");
            
            if (cameraManager == null)
            {
                Debug.LogError("CameraManager reference is null!");
                return;
            }
            
            cameraManager.SetConversationView(_isOpen);
        }
    }

    void MovePlayer()
    {
        if (moveDirection.magnitude >= 0.1f)
        {
            Vector3 movement = moveDirection * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}