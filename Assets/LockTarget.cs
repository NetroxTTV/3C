using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class LockTarget : MonoBehaviour
{
    [SerializeField] InputActionReference _lock;
    [SerializeField] CinemachineTargetGroup _targetGroup;
    [SerializeField] Transform _target;
    
    bool isLocked;    
    
    void Start()
    {
        _lock.action.started += ToggleLock;
    }
    void OnDestroy()
    {
        _lock.action.started -= ToggleLock;
    }

    void ToggleLock(InputAction.CallbackContext obj)
    {
        if (isLocked)
        {
            _targetGroup.RemoveMember(_target);
        }
        else
        {
            _targetGroup.AddMember(_target, 1, 1);
        }
        isLocked = !isLocked;        
    }
}
