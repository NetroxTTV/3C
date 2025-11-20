using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class Explosion : MonoBehaviour
{
    [SerializeField] InputActionReference _explosion;
    [SerializeField] CinemachineImpulseSource _impulse;
    
    void Start()
    {
        _explosion.action.started += LaunchExplosion;
    }

    void OnDestroy()
    {
        _explosion.action.started -= LaunchExplosion;
    }

    void LaunchExplosion(InputAction.CallbackContext obj)
    {
        _impulse.GenerateImpulse();
    }
}
