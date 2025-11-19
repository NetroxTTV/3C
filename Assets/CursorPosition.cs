using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class CursorPosition : MonoBehaviour
{
    [SerializeField] Camera _cam;
    [SerializeField] InputActionReference _mousePosition;

    void Update()
    {
        var pos = _mousePosition.action.ReadValue<Vector2>();
        
       var t = _cam.ScreenToWorldPoint(pos);
       t.z = 0;
       transform.position = t;
    }
}
