using UnityEngine;
using UnityEngine.InputSystem;

public class EntityDialogue : MonoBehaviour
{
    [SerializeField] InputActionProperty _dialogue;
    [SerializeField] Transform _cam;
    
    void Start()
    {
        _dialogue.action.Enable();
        _dialogue.action.started += ToggleDialogueCam;
    }

    void ToggleDialogueCam(InputAction.CallbackContext obj)
    {
        _cam.gameObject.SetActive(!_cam.gameObject.activeSelf);
    }
}
