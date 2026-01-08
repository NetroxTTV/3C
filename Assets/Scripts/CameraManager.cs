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
        Debug.Log("SetConversationView called with: " + a);
        
        conversationCamera.Priority = 15;
        normalCamera.Priority = 10;

        if (dialogueCanvas != null)
        {
            Debug.Log("Setting dialogue canvas active to: " + a);
            dialogueCanvas.gameObject.SetActive(a);

            if (a == false)
            {
                conversationCamera.Priority = 10;
                normalCamera.Priority = 15;
            }
        }
        else
        {
            Debug.LogError("Dialogue canvas is null!");
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