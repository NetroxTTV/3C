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

    public void SetConversationView(bool a)
    {
        conversationCamera.Priority = 15;
        normalCamera.Priority = 10;

        if (dialogueCanvas != null)
        {
            dialogueCanvas.gameObject.SetActive(a);
            _isOpen = a;

            if (a == false)
            {
                conversationCamera.Priority = 10;
                normalCamera.Priority = 15;
            }
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