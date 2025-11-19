using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class EntityMove : MonoBehaviour
{
    [SerializeField] InputActionProperty _move;
    [SerializeField] float _speed;
    [SerializeField] Camera _camera;
    [SerializeField] bool _force2D;

    
    Vector3 _direction;
    
    void Start()
    {
        _move.action.Enable();
        _move.action.performed += StartMove;
        _move.action.canceled += StopMove;
    }

    void OnDestroy()
    {
        _move.action.performed -= StartMove;
        _move.action.canceled -= StopMove;
    }

    void StartMove(InputAction.CallbackContext ctx)
    {
        var d = ctx.ReadValue<Vector2>();
        _direction = new Vector3(d.x, 0, d.y);
    }

    void StopMove(InputAction.CallbackContext ctx)
    {
        _direction= Vector3.zero;
    }

    void Update()
    {
        Vector3 finalDirection = _direction;
        if (_camera != null)
        {
            var forward = new Vector3(_camera.transform.forward.x,0,_camera.transform.forward.z).normalized;
            var right = new Vector3(_camera.transform.right.x,0,_camera.transform.right.z).normalized;
            
            finalDirection = forward * _direction.z + right * _direction.x;
        }

        if (_force2D)
        {
            finalDirection.y = finalDirection.z;
            finalDirection.z = 0;
        }
        transform.parent.Translate(finalDirection * _speed * Time.deltaTime);

        if (!_force2D)
        {
            transform.LookAt(transform.position + finalDirection);
        }
    }
}
