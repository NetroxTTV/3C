using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [Header("Cinemachine Cameras")]
    public CinemachineCamera normalCamera;
    public CinemachineCamera conversationCamera;

    [Header("UI")]
    public Canvas dialogueCanvas;

    void Start()
    {
        SetNormalView();
    }

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            SetConversationView();
        }
    }

    public void SetConversationView()
    {
        conversationCamera.Priority = 15;
        normalCamera.Priority = 10;

        if (dialogueCanvas != null)
        {
            dialogueCanvas.gameObject.SetActive(true);
        }
    }

    public void SetNormalView()
    {
        conversationCamera.Priority = 5;
        normalCamera.Priority = 10;

        if (dialogueCanvas != null)
        {
            dialogueCanvas.gameObject.SetActive(false);
        }
    }
}