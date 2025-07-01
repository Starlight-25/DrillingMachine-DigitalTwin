using System.Collections.Generic;
using UnityEngine;

public class ReplayParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem Particles;
    private ReplayDMMovements ReplayDMMovements;
    [SerializeField] private DrillingDataManager DrillingDataManager;
    private List<DrillingDataCSV> DrillingData;

    
    
    
    
    private void Start()
    {
        ReplayDMMovements = GetComponent<ReplayDMMovements>();
        DrillingData = DrillingDataManager.DrillingData;
    }

    
    
    
    
    private void Update()
    {
        if (!ReplayDMMovements.IsPaused() && DrillingData[DrillingDataManager.Index].DrillBit_Rotation > 0.01) Particles.Play();
        else Particles.Stop();
    }
}