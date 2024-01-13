using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ParryEffect : MonoBehaviour, IFX
{
    private ParticleSystem _particleSystem;
    private void Awake()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();  
    }
    public void Play()
    {
        _particleSystem.Play();
    }
}
