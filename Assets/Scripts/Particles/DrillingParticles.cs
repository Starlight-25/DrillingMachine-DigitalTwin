using System;
using UnityEngine;

public class DrillingParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem Particles;
    private DrillingMachineMovements DrillingMachineMovements;

    
    
    
    
    private void Start() => DrillingMachineMovements = GetComponent<DrillingMachineMovements>();
    

    
    
    
    
    private void Update()
    {
        if (DrillingMachineMovements.GetIsDrilling()) Particles.Play();
        else Particles.Stop();
    }
}