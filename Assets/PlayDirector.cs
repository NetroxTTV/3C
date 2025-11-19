using System;
using UnityEngine;
using UnityEngine.Playables;

public class PlayDirector : MonoBehaviour
{
    [SerializeField] PlayableDirector _playableDirector;
    
    
    void OnTriggerEnter(Collider other)
    {
        if(other.attachedRigidbody.CompareTag("Player")) _playableDirector.Play();
            
    }
}
